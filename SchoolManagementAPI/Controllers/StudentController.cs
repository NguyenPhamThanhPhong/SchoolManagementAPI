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
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly EmailUtil _emailUtil;


        public StudentController(IStudentRepository studentRepository, IMapper mapper, EmailUtil emailUtil)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _emailUtil = emailUtil;
        }

        [HttpPost("/student-create")]
        public async Task<IActionResult> Create([FromBody] SchoolMemberCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = _mapper.Map<Student>(request);
            await _studentRepository.Create(student);
            return Ok(student);
        }
        [HttpGet("/student-get-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyId(id);
            return Ok(student);
        }
        [HttpPost("/student-login/")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyUsername(request.Username);
            if (student == null || student.Password != request.Password)
                return BadRequest("not found username");
            return Ok(student);
        }
        [HttpGet("student-get-password-in-mail/{username}")]
        public async Task<IActionResult> RecoverPassword(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyUsername(username);
            if (student == null)
                return BadRequest("not found username");
            if (student.Email == null)
                return BadRequest("user's email doesn't exist ");

            var isSent = await _emailUtil.SendEmailAsync(student.Email, "no-reply: your password is", "this is your password" + student.Password);

            return Ok(isSent);
        }
        [HttpGet("/student-get-many-range/{start}/{end}")]
        public async Task<IActionResult> ManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetManyRange(start, end);
            return Ok(students);
        }
        [HttpPost("/student-get-many-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetManyfromIds(ids);
            return Ok(students);
        }
        [HttpPost("/student-get-by-text-filter")]
        public async Task<IActionResult> GetbyTextFilter([FromForm] string filter)
        {
            Console.WriteLine("in controller: \n"+ filter+"\n");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetbyTextFilter(filter);
            return Ok(students);
        }
        [HttpDelete("/student-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isDeleted = await _studentRepository.Delete(id);
            return Ok(isDeleted);
        }
        [HttpPost("/student-update-string-fields/{id}")]
        public async Task<IActionResult> UpdateStringFields(string id, [FromBody] List<UpdateParameter> paramters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isUpdated = await _studentRepository.UpdateStringFields(id, paramters);
            return Ok(isUpdated);
        }
        [HttpPost("/student-update-instance")]
        public async Task<IActionResult> UpdateStringFields([FromBody] Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isUpdated = await _studentRepository.UpdatebyInstance(student.ID,student);
            if (!isUpdated)
                return BadRequest(isUpdated);
            return Ok(student);
        }
    }
}
