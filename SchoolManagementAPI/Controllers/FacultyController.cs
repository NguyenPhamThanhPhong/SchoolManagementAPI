using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMongoCollection<Faculty> _facultyCollection;

        public FacultyController(IFacultyRepository facultyRepository,DatabaseConfig databaseConfig)
        {
            _facultyRepository = facultyRepository;
            _facultyCollection = databaseConfig.FacultyCollection;
        }
        [HttpPost("/faculty-create")]
        public async Task<IActionResult> Create([FromBody] Faculty faculty)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _facultyRepository.Create(faculty);
            return Ok(faculty);
        }
        [HttpPost("/faculty-auto-generate")]
        public async Task<IActionResult> AutoGenerate([FromBody] List<Faculty> faculties)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _facultyCollection.InsertManyAsync(faculties);
            return Ok(faculties);
        }


        [HttpGet("/faculty-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var facultys = await _facultyRepository.GetAll();
            return Ok(facultys);
        }
        [HttpGet("/faculty-get-by-id/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var faculty = await _facultyRepository.GetOne(id);
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
                bool isDeleted = await _facultyRepository.Delete(id);
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("/faculty-delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody]List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<Faculty>.Filter.In(f => f.ID, ids);
            var result = await _facultyCollection.DeleteManyAsync(filter);
            return Ok(result.DeletedCount);
        }
    }
}
