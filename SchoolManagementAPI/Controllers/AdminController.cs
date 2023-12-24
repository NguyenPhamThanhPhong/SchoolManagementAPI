using Amazon.Runtime.Internal;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.SMTP;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly EmailUtil _emailUtil;

        public AdminController(IAdminRepository adminRepository, IMapper mapper, EmailUtil emailUtil)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            this._emailUtil = emailUtil;
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
            return Ok(admin);
        }
        [HttpGet("/admin-get-password-in-mail/{username}")]
        public async Task<IActionResult> Create(string username)
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
