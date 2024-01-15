using Amazon.Runtime.Internal;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.CloudinaryService;
using SchoolManagementAPI.Services.Configs;
using System.Reflection.Metadata;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolClassController : ControllerBase
    {
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IMapper _mapper;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _schoolClassFolderName;
        private readonly IMongoCollection<SchoolClass> _schoolClassCollection;
        private readonly IMongoCollection<Student> _studentCollection;

        public SchoolClassController(ISchoolClassRepository schoolClassRepository, IMapper mapper, DatabaseConfig databaseConfig,
            CloudinaryHandler cloudinaryHandler, CloudinaryConfig cloudinaryConfig)
        {
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
            _cloudinaryHandler = cloudinaryHandler;
            _schoolClassFolderName = cloudinaryConfig.ClassSectionFolderName;
            _schoolClassCollection = databaseConfig.SchoolClassCollection;
            _studentCollection = databaseConfig.StudentCollection;
        }

        [HttpGet("/class-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schoolClass = await _schoolClassRepository.GetAll();
            return Ok(schoolClass);
        }

        [HttpPost("/class-create")]
        public async Task<IActionResult> Create([FromBody] SchoolClassCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var schoolClass = _mapper.Map<SchoolClass>(request);
            await _schoolClassRepository.Create(schoolClass);
            return Ok(schoolClass);
        }

        [HttpDelete("/class-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var deleteResult = await _schoolClassRepository.Delete(id);

            if (deleteResult!=null)
                return Ok($"deleted {deleteResult}");
            return BadRequest(deleteResult);
        }
        [HttpDelete("/class-delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filter = Builders<SchoolClass>.Filter.In(s => s.ID, ids);
            var deleteResult = await _schoolClassCollection.DeleteManyAsync(filter);
            return Ok(deleteResult.DeletedCount>0);
        }

        [HttpGet("/class-get-by-id/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var schoolclass = await _schoolClassRepository.GetSingle(id);
            if (schoolclass != null)
                return Ok(schoolclass);
            return Ok(schoolclass);
        }
        [HttpPost("/class-get-many-from-ids/")]
        public async Task<IActionResult> GetfromIds(List<string> ids)
        {
            var classes = await _schoolClassRepository.GetfromIds(ids);
            return Ok(classes);
        }

        [HttpGet("/class-get-many-range/{start}/{end}")]
        public async Task<IActionResult> GetManyRange(int start, int end)
        {
            var classes = await _schoolClassRepository.GetManyRange(start, end);
            return Ok(classes);
        }


        [HttpPost("/class-update-instance")]
        public async Task<IActionResult> UpdateInstance([FromBody] SchoolClass schoolClass)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _schoolClassRepository.UpdatebyInstance(schoolClass);
            return Ok(schoolClass);
        }
        [HttpPost("/class-student-registration")]
        public async Task<IActionResult> UpdateStudentRegistration([FromBody] SchoolClassRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID , request.ID);
            if (request.option == UpdateOption.push)
            {
                var studentlog = new StudentLog { ID = request.StudentId };
                var update = Builders<SchoolClass>.Update.Push(s => s.StudentLogs, studentlog);
                var isupdated = await _schoolClassCollection.UpdateOneAsync(filter, update);
                var updateStudent = Builders<Student>.Update.Push(s => s.Classes, request.ID);
                var isupdate2 = await _studentCollection.UpdateOneAsync(s => s.ID == request.StudentId, updateStudent);
                Console.WriteLine("run here" + isupdated.ModifiedCount);
                Console.WriteLine("rrun 333" + isupdate2.ModifiedCount);
            }
            else
            {

                var update = Builders<SchoolClass>.Update.PullFilter(s => s.StudentLogs, Builders<StudentLog>.Filter.Eq(stu => stu.ID, request.StudentId));
                await _schoolClassCollection.UpdateOneAsync(filter, update);
                var updateStudent = Builders<Student>.Update.Pull(s => s.Classes, request.ID);
                await _studentCollection.UpdateOneAsync(s => s.ID == request.StudentId, updateStudent);
            }


            return Ok("updated");
        }
        [HttpPost("/class-update-create-fields/{id}")]
        public async Task<IActionResult> UpdateCreatFields([FromBody] SchoolClassCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var schoolClass = await _schoolClassCollection.Find(s => s.ID == request.ID).FirstOrDefaultAsync();
            schoolClass.Name = request.Name;
            schoolClass.RoomName = request.RoomName;
            schoolClass.Program = request.Program;
            schoolClass.ClassType = request.ClassType;
            schoolClass.Subject = request.Subject;
            schoolClass.SemesterId = request.SemesterId;
            schoolClass.Lecturer = request.Lecturer;
            schoolClass.Schedule = request.Schedule;
            await _schoolClassCollection.ReplaceOneAsync(s => s.ID == request.ID, schoolClass);
            return Ok("updated");
        }
        [HttpPost("/class-update-exam/{id}")]
        public async Task<IActionResult> UpdateExams([FromQuery]string id,[FromBody] List<ExamMileStone> exams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, id);
            var update = Builders<SchoolClass>.Update.Set(s => s.Exams, exams);
            await _schoolClassCollection.UpdateOneAsync(filter, update);
            return Ok("updated");
        }
        [HttpPost("/class-update-")]

        [HttpPost("/class-append-sections/{id}/{position}/{updateOption}")]
        public async Task<IActionResult> AppendSection(string id, int position, UpdateOption option, [FromForm] SchoolClassUpdateSectionsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var section = _mapper.Map<Section>(request);
            var isUpdated = false;
            try
            {
                switch (option)
                {
                    case UpdateOption.set:
                        {
                            var filter = Builders<SchoolClass>.Filter.And(
                            Builders<SchoolClass>.Filter.Eq(c => c.ID, id),
                            Builders<SchoolClass>.Filter.ElemMatch(c => c.Sections, s => s.Position == position));
                            foreach (var url in request.PrevUrls)
                                await _cloudinaryHandler.Delete(url);
                            section.DocumentUrls = await _cloudinaryHandler.UploadImages(request.FormFiles, _schoolClassFolderName);
                            var update = Builders<SchoolClass>.Update.Set(s => s.Sections[-1], section);
                            isUpdated = await _schoolClassRepository.UpdatebyFilter(filter, update, false);
                        }
                        break;
                    case UpdateOption.pull:
                        {
                            var filter = Builders<SchoolClass>.Filter.Eq(c => c.ID, id);
                            var update = Builders<SchoolClass>.Update.PullFilter(c => c.Sections, s => s.Position == position);
                            await _schoolClassRepository.UpdatebyFilter(filter, update, false);
                        }
                        break;
                    case UpdateOption.push:
                        {
                            var filter = Builders<SchoolClass>.Filter.And(
                            Builders<SchoolClass>.Filter.Eq(c => c.ID, id));
                            var update = Builders<SchoolClass>.Update.Push(c => c.Sections, section);
                            await _schoolClassRepository.UpdatebyFilter(filter, update, false);
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                BadRequest(new { isUpdated = false, errorMessage = ex.Message });
            }
            return Ok(isUpdated);
        }
    }
}
