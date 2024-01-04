using Amazon.Runtime.Internal;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Authentication;
using SchoolManagementAPI.Services.SMTP;
using System.Security.Claims;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IMapper _mapper;
        private readonly EmailUtil _emailUtil;
        private readonly TokenGenerator _tokenGenerator;

        public AdminController(IAdminRepository adminRepository, IMapper mapper, EmailUtil emailUtil, TokenGenerator tokenGenerator, IStudentRepository studentRepository, ILecturerRepository lecturerRepository)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            this._emailUtil = emailUtil;
            _tokenGenerator = tokenGenerator;
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
        }

        [HttpPost("/admin-create")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateRequest request )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var admin = _mapper.Map<Admin>(request);
            await _adminRepository.Create(admin);
            return Ok(admin);
        }
        [HttpPost("/admin-login/")]
        public async Task<IActionResult> Create([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var admin = await _adminRepository.GetbyUsername(request.Username);
            if (admin == null || admin.Password != request.Password)
                return BadRequest("not found username");
            var accessToken = _tokenGenerator.GenerateAccessToken(admin);
            return Ok(new {account=admin,accessToken=accessToken});
        }
        //[Authorize(Roles ="admin")]
        [HttpPost("/admin-auto-login")]
        public async Task<IActionResult> GetAuthorizedData([FromBody] string token)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine(role);
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (id == null || username == null)
                return BadRequest("not found user");
            switch (role)
            {
                case "admin":
                    var admin = await _adminRepository.GetbyID(id);
                    return Ok(admin);
                case "lecturer":
                    var lecturer = await _lecturerRepository.GetbyUsername(username);
                    return Ok(lecturer);
                case "student":
                    var student = await _studentRepository.GetbyId(id);
                    return Ok(student);
                default:
                    return BadRequest("not found user");
            }
        }

        [HttpGet("/admin-get-password-in-mail/{username}")]
        public async Task<IActionResult> RecoverPassword(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var admin = await _adminRepository.GetbyUsername(username);
            if (admin == null)
                return BadRequest("not found username");
            if (admin.Email == null)
                return BadRequest("user's email doesn't exist ");

            var isSent = await _emailUtil.SendEmailAsync(admin.Email, "no-reply: your password is", "this is your password" + admin.Password);

            return Ok(isSent);
        }

        [HttpGet("/admin-get-all")]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _adminRepository.GetAll();
            return Ok(admins);
        }
    }
}
