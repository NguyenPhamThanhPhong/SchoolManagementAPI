using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRegistrationController : ControllerBase
    {
        private readonly IMongoCollection<StudentRegistration> _studentRegistrationCollection;
        public StudentRegistrationController(DatabaseConfig config)
        {
            _studentRegistrationCollection = config.StudentRegistrationCollection;
        }

        [HttpPost("/registration-create")]
        public async Task<IActionResult> Create([FromBody] StudentRegistration registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
             await _studentRegistrationCollection.InsertOneAsync(registration);
            return Ok(registration);
        }
        [HttpPost("/registration-update-instance")]
        public async Task<IActionResult> Update([FromBody] StudentRegistration registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _studentRegistrationCollection.ReplaceOneAsync(s=>s.ID==registration.ID,registration);
            return Ok(registration);
        }
        [HttpDelete("/registration-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _studentRegistrationCollection.DeleteOneAsync(s=>s.ID==id);
            return Ok("deleted");
        }
        [HttpGet("/registration-get-all")]
        public async Task<IActionResult> Getall()
        {
            if (!ModelState.IsValid)
                return BadRequest();
            List<StudentRegistration> registrations = await _studentRegistrationCollection.Find(_ => true).ToListAsync();
            return Ok(registrations);
        }
    }
}
