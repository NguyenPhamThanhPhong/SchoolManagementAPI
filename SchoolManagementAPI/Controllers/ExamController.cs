using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IMongoCollection<SchoolClass> _schoolClassCollection;
        private readonly IMongoCollection<Student> _studentCollection;
        private readonly FindOneAndUpdateOptions<SchoolClass> _schoolClassOptions;
        private readonly IMapper _mapper;

        public ExamController(IMapper mapper, DatabaseConfig databaseConfig)
        {
            _mapper = mapper;
            _schoolClassCollection = databaseConfig.SchoolClassCollection;
            _studentCollection = databaseConfig.StudentCollection;
            _schoolClassOptions = new FindOneAndUpdateOptions<SchoolClass>
            {
                ReturnDocument = ReturnDocument.After, 
            };
        }

        [HttpPost("/create-exam/{classId}")]
        public async Task<IActionResult> Create(string classId, [FromBody] ExamMileStone exam)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            exam.Id = Guid.NewGuid().ToString();
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, classId);
            var update = Builders<SchoolClass>.Update.Push(s => s.Exams, exam);
            var result = await _schoolClassCollection.FindOneAndUpdateAsync(filter, update,_schoolClassOptions);
            return Ok(result);
        }
        [HttpDelete("/delete-exam/{classId}/{examId}")]
        public async Task<IActionResult> Delete(string classId, string examId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, classId);
            var update = Builders<SchoolClass>.Update
                .PullFilter(s => s.Exams, Builders<ExamMileStone>.Filter.Eq(e => e.Id, examId));
            var result = await _schoolClassCollection.FindOneAndUpdateAsync(filter, update, _schoolClassOptions);
            return Ok(result);
        }
        [HttpPost("/update-exam/{classId}")]
        public async Task<IActionResult> Update(string classId, [FromBody] List<ExamMileStone> exams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, classId);
            var update = Builders<SchoolClass>.Update.Set(s => s.Exams, exams);
            var result = await _schoolClassCollection.FindOneAndUpdateAsync(filter, update, _schoolClassOptions);
            return Ok(result);
        }

        [HttpPost("/save-scores/{classId}")]
        public async Task<IActionResult> SaveScore(string classId, [FromBody] List<StudentItem> studentItems)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, classId);
            var update = Builders<SchoolClass>.Update.Set(s => s.StudentItems, studentItems);
            var result = await _schoolClassCollection.FindOneAndUpdateAsync(filter, update, _schoolClassOptions);
            return Ok(result);
        }
        [HttpPost("/submit-scores/{classId}")]
        public async Task<IActionResult> SubmitScores(string classId, [FromBody] List<StudentItem> studentItems)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, classId);
            var update = Builders<SchoolClass>.Update.Set(s => s.StudentItems, studentItems);
            var schoolClass = await _schoolClassCollection.FindOneAndUpdateAsync(filter, update,_schoolClassOptions);
            List<string> studentIds = studentItems.Select(s => s.Id).ToList();
            List<Task> updateStudentTasks = new List<Task>();

            foreach (var studentRow in studentItems)
            {
                CreditLog creditLog = new CreditLog()
                {
                    Id = classId,
                    Name = schoolClass?.Name ?? "",
                    SemesterId = schoolClass?.SemesterId,
                    Progress = studentRow.Progress,
                    Midterm = studentRow.Midterm,
                    Practice = studentRow.Practice,
                    Final = studentRow.Final
                };

                var filterStudent = Builders<Student>.Filter.Eq(s => s.ID, studentRow.Id);
                var updateStudentPull = Builders<Student>.Update.PullFilter(s => s.creditLogs, Builders<CreditLog>.Filter.Eq(s => s.Id, classId));
                var updateStudentPush = Builders<Student>.Update.Push(s => s.creditLogs, creditLog);

                updateStudentTasks.Add(Task.Run(async () =>
                {
                    await _studentCollection.UpdateOneAsync(filterStudent, updateStudentPull);
                    await _studentCollection.UpdateOneAsync(filterStudent, updateStudentPush);
                }));
            }
            await Task.WhenAll(updateStudentTasks);

            return Ok(schoolClass);
        }
    }
}
