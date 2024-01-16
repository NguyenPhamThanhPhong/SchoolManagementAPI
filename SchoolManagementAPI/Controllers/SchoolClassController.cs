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
using System.Text.Json;

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
        private readonly FindOneAndUpdateOptions<SchoolClass> _schoolClassOptions;


        public SchoolClassController(ISchoolClassRepository schoolClassRepository, IMapper mapper, DatabaseConfig databaseConfig,
            CloudinaryHandler cloudinaryHandler, CloudinaryConfig cloudinaryConfig)
        {
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
            _cloudinaryHandler = cloudinaryHandler;
            _schoolClassFolderName = cloudinaryConfig.ClassSectionFolderName;
            _schoolClassCollection = databaseConfig.SchoolClassCollection;
            _studentCollection = databaseConfig.StudentCollection;
            _schoolClassOptions = new FindOneAndUpdateOptions<SchoolClass>
            {
                ReturnDocument = ReturnDocument.After,
            };
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

            if (deleteResult != null)
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
            return Ok(deleteResult.DeletedCount > 0);
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
            string studentId = request.StudentId;
            string classId = request.ID;
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, classId);
            var filterStudent = Builders<Student>.Filter.Eq(s => s.ID, studentId);
            var studentItem = new StudentItem { Id = studentId, Name = request.Name };
            if (request.option == UpdateOption.push)
            {

                var update = Builders<SchoolClass>.Update.AddToSet(s => s.StudentItems, studentItem);
                var updateStudent = Builders<Student>.Update.AddToSet(s => s.Classes, classId);
                await Task.WhenAll(
                    _schoolClassCollection.UpdateOneAsync(filter, update),
                    _studentCollection.UpdateOneAsync(filterStudent, updateStudent));
            }
            else
            {
                var updatePull = Builders<SchoolClass>.Update.PullFilter(s => s.StudentItems, Builders<StudentItem>.Filter.Eq(stu => stu.Id, studentId));
                var updatePush = Builders<SchoolClass>.Update.Push(s => s.StudentItems, studentItem);

                var updateStudent = Builders<Student>.Update.Pull(s => s.Classes, classId);

                await Task.WhenAll(
                    _schoolClassCollection.UpdateOneAsync(filter, updatePull),
                    _schoolClassCollection.UpdateOneAsync(filter, updatePush),
                    _studentCollection.UpdateOneAsync(filterStudent, updateStudent));
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
        public async Task<IActionResult> UpdateExams([FromQuery] string id, [FromBody] List<ExamMileStone> exams)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, id);
            var update = Builders<SchoolClass>.Update.Set(s => s.Exams, exams);
            await _schoolClassCollection.UpdateOneAsync(filter, update);
            return Ok("updated");
        }


        [HttpPost("class-update-sections/{id}/{index}")]
        public async Task<IActionResult> UpdateSection(string id, int index, [FromForm] SchoolClassUpdateSectionsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (index != -1)
            {
                var section = request.Sections[index];
                section.DocumentUrls = request.PrevUrls;
                if (request.FormFiles != null && request.FormFiles.Count > 0)
                {
                    var uploadResult = await _cloudinaryHandler.UploadImages(request.FormFiles, _schoolClassFolderName);
                    if (section.DocumentUrls != null)
                        foreach (var item in uploadResult)
                            section.DocumentUrls.Add(item.Key, item.Value);
                    else
                        section.DocumentUrls = uploadResult;
                }
            }
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, id);
            var update = Builders<SchoolClass>.Update.Set(s => s.Sections, request.Sections);

            var result = await _schoolClassCollection.FindOneAndUpdateAsync(filter, update, _schoolClassOptions);
            return Ok(result);
        }
    }
}
