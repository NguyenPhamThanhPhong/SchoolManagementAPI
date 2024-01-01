using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        private readonly ISchoolClassRepository _schoolClassRepository;

        public SubjectController(ISubjectRepository subjectRepository, IMapper mapper, ISchoolClassRepository schoolClassRepository)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
            _schoolClassRepository = schoolClassRepository;
        }

        [HttpPost("/subject-create")]
        public async Task<IActionResult> Create([FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _subjectRepository.Create(subject);
            return Ok(subject);
        }
        [HttpGet("/subject-get-many-range/{start}/{end}")]
        public async Task<IActionResult> GetManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjects = await _subjectRepository.GetManyRange(start, end);
            return Ok(subjects);
        }
        [HttpGet("/subject-get-by-id/{id}")]
        public async Task<IActionResult> GetbyId(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subject = await _subjectRepository.GetOne(id);
            return Ok(subject);
        }
        [HttpPost("/subject-update-instance/{prevName}")]
        public async Task<IActionResult> UpdateInstance(string prevName, [FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Task? updateSubject = _subjectRepository.UpdatebyInstance(subject);
            Task? myTask = Task.Run(async () =>
            {
                if (prevName != subject.Name)
                {
                    var filter = Builders<SchoolClass>.Filter.In(s => s.ID, subject.ClassIds);
                    var update = Builders<SchoolClass>.Update.Set(s => s.Subject, new DataLink { ID = subject.ID, Name = subject.Name });
                    await _schoolClassRepository.UpdatebyFilter(filter, update, true);
                }
            });
            await Task.WhenAll(updateSubject,myTask);
            return Ok(subject);
        }
        [HttpDelete("/subject-delete/{id}")]
        public async Task<IActionResult> Delete( string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _subjectRepository.DeleteOne(id);
            return Ok("deleted");
        }

    }
}
