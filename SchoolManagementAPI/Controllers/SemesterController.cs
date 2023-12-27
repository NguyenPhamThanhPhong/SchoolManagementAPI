using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterController(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }
        [HttpGet("/semester-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var semesters = await _semesterRepository.GetAll();
            return Ok(semesters);
        }
        [HttpGet("/semester/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var semester = await _semesterRepository.GetOne(id);
            return Ok(semester);
        }
        [HttpPost("/semester-create")]
        public async Task<IActionResult> Create([FromBody] Semester semester)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _semesterRepository.Create(semester);
            return Ok(semester);
        }
        [HttpPost("/semester-update")]
        public async Task<IActionResult> Update([FromBody] Semester semester)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _semesterRepository.UpdatebyInstance(semester);
            return Ok(semester);
        }
        [HttpDelete("/semester-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _semesterRepository.Delete(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("deleted");
        }
    }
}
