using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyRepository _facultyRepository;

        public FacultyController(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }
        [HttpGet("/faculty-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var facultys = await _facultyRepository.GetAll();
            return Ok(facultys);
        }
        [HttpGet("/faculty/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var faculty = await _facultyRepository.GetOne(id);
            return Ok(faculty);
        }
        [HttpPost("/faculty-create")]
        public async Task<IActionResult> Create([FromBody] Faculty faculty)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _facultyRepository.Create(faculty);
            return Ok(faculty);
        }
        [HttpPost("/faculty-update")]
        public async Task<IActionResult> Update([FromBody] Faculty faculty)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _facultyRepository.UpdatebyInstance(faculty);
            return Ok(faculty);
        }
        [HttpDelete("/faculty-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _facultyRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("deleted");
        }
    }
}
