using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;
using System.Text.Json;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMongoCollection<Semester> _semesterCollection;   

        public SemesterController(ISemesterRepository semesterRepository,DatabaseConfig databaseConfig)
        {
            _semesterRepository = semesterRepository;
            _semesterCollection = databaseConfig.SemesterCollection;
        }
        [HttpGet("/semester-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var semesters = await _semesterRepository.GetAll();
            return Ok(semesters);
        }
        [HttpGet("/semester-get-by-id/{id}")]
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
            Console.WriteLine(semester.StartTime.ToString("dd/MM/yyyy"));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _semesterRepository.Create(semester);
            return Ok(semester);
        }
        [HttpPost("/semester-auto-generate")]
        public async Task<IActionResult> AutoGenerate([FromBody] List<Semester> semesters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _semesterCollection.InsertManyAsync(semesters);
            return Ok(semesters);
        }



        [HttpPost("/semester-update")]
        public async Task<IActionResult> Update([FromBody] Semester semester)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Console.WriteLine(JsonSerializer.Serialize(semester));
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
        [HttpDelete("/semester-delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var filter = Builders<Semester>.Filter.In(s => s.ID, ids);
                var result = await _semesterCollection.DeleteManyAsync(filter);
                return Ok(result.DeletedCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
