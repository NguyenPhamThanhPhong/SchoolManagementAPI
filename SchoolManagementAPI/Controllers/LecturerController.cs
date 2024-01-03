using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
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
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IMapper _mapper;
        private readonly EmailUtil _emailUtil;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly TokenGenerator _tokenGenerator;


        public LecturerController(ILecturerRepository lecturerRepository, IMapper mapper, EmailUtil emailUtil, ISchoolClassRepository schoolClassRepository, TokenGenerator tokenGenerator)
        {
            _lecturerRepository = lecturerRepository;
            _mapper = mapper;
            _emailUtil = emailUtil;
            _schoolClassRepository = schoolClassRepository;
            _tokenGenerator = tokenGenerator;
        }
        [HttpPost("/lecturer-create")]
        public async Task<IActionResult> Create([FromBody] SchoolMemberCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturer = _mapper.Map<Lecturer>(request);
            await _lecturerRepository.Create(lecturer);

            return Ok(lecturer);
        }
        [HttpPost("/lecturer-login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturer = await _lecturerRepository.GetbyUsername(request.Username);
            if (lecturer == null || lecturer.Password != request.Password)
                return BadRequest("not found username");
            var accessToken = _tokenGenerator.GenerateAccessToken(lecturer);
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
            });
            return Ok(lecturer);
        }
        [HttpGet("/request-logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok("logged out");
        }

        [HttpGet("lecturer-get-password-in-mail/{username}")]
        public async Task<IActionResult> RecoverPassword(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturer = await _lecturerRepository.GetbyUsername(username);
            if (lecturer == null)
                return BadRequest("not found username");
            if (lecturer.Email == null)
                return BadRequest("user's email doesn't exist ");

            var isSent = await _emailUtil.SendEmailAsync(lecturer.Email, "no-reply: your password is", "this is your password" + lecturer.Password);

            return Ok(isSent);
        }
        [HttpGet("/lecturer-get-many-range/{start}/{end}")]
        public async Task<IActionResult> ManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturers = await _lecturerRepository.GetManyRange(start, end);
            return Ok(lecturers);
        }
        [HttpPost("/lecturer-get-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturers = await _lecturerRepository.GetManyfromIds(ids);
            return Ok(lecturers);
        }
        [HttpPost("/lecturer-get-by-text-filter")]
        public async Task<IActionResult> GetbyTextFilter([FromForm] string filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturers = await _lecturerRepository.GetbyTextFilter(filter);
            return Ok(lecturers);
        }
        [HttpDelete("/lecturer-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isDeleted = await _lecturerRepository.Delete(id);
            return Ok(isDeleted);
        }
        [HttpPost("/lecturer-update-string-fields/{id}")]
        public async Task<IActionResult> UpdateStringFields(string id, [FromBody] List<UpdateParameter> paramters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isUpdated = await _lecturerRepository.UpdateStringFields(id, paramters);
            return Ok(isUpdated);
        }
        [HttpPost("/lecturer-update-instance/{id}/{prevName}")]
        public async Task<IActionResult> UpdateStringFields(string id,string? prevName, [FromBody] Lecturer lecturer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isUpdated = await _lecturerRepository.UpdatebyInstance(lecturer);
            if(prevName!=lecturer.PersonalInfo.Name)
            {
                var filter = Builders<SchoolClass>.Filter.In(c=>c.ID,lecturer.Classes);
                var update = Builders<SchoolClass>.Update.Set(c => c.Lecturer.Name, lecturer.PersonalInfo.Name);
                await _schoolClassRepository.UpdatebyFilter(filter, update,true);
            }

            if (!isUpdated)
                return BadRequest(isUpdated);
            return Ok(lecturer);
        }
    }
}
