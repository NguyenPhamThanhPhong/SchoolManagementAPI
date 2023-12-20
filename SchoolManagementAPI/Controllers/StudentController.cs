using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpPost("/student-create")]
        public async Task<IActionResult> Create([FromBody] StudentCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = _mapper.Map<Student>(request);
            await _studentRepository.Create(student);
            return Ok(student);
        }
        [HttpPost("/student-login/")]
        public async Task<IActionResult> Create([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyUsername(request.Username);
            if (student == null || student.Password != request.Password)
                return BadRequest("not found username");
            return Ok(student);
        }
        [HttpGet("/student-get-all/{start}/{end}")]
        public async Task<IActionResult> ManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetManyRange(start, end);
            return Ok(students);
        }
        [HttpPost("/student-get-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetManyfromIds(ids);
            return Ok(students);
        }
        [HttpPost("/student-get-by-text-filter")]
        public async Task<IActionResult> GetbyTextFilter([FromBody] string filter)
        {
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
        public async Task<IActionResult> UpdateStringFields(string id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isUpdated = await _studentRepository.UpdatebyInstance(id,student);
            if (!isUpdated)
                return BadRequest(isUpdated);
            return Ok(student);
        }
    }
}
