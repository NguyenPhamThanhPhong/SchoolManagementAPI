using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Authentication;
using SchoolManagementAPI.Services.CloudinaryService;
using SchoolManagementAPI.Services.Configs;
using SchoolManagementAPI.Services.Converters;
using SchoolManagementAPI.Services.SMTP;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly EmailUtil _emailUtil;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IMongoCollection<Student> _studentCollection;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _studentFolderName;

        public StudentController(IStudentRepository studentRepository, IMapper mapper,
            EmailUtil emailUtil, TokenGenerator tokenGenerator, DatabaseConfig databaseConfig, 
            CloudinaryHandler cloudinaryHandler,CloudinaryConfig cloudinaryConfig)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _emailUtil = emailUtil;
            _tokenGenerator = tokenGenerator;
            _studentCollection = databaseConfig.StudentCollection;
            _cloudinaryHandler = cloudinaryHandler;
            _studentFolderName = cloudinaryConfig.StudentFolderName;
        }

        [HttpPost("/student-create")]
        public async Task<IActionResult> Create([FromForm] FormDataRequest formDataRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryDeserializeJson(formDataRequest.Requestbody, out SchoolMemberCreateRequest? request))
                return BadRequest(request);
            if (request == null)
                return BadRequest(ModelState);

            var student = _mapper.Map<Student>(request);
            if (formDataRequest?.File != null)
            {
                string? url = await _cloudinaryHandler.UploadSingleImage(formDataRequest.File, _studentFolderName);
                student.PersonalInfo.AvatarUrl = url;
            }
            await _studentRepository.Create(student);
            return Ok(student);
        }

        [HttpGet("/student-get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = await _studentRepository.GetAll();
            return Ok(student);
        }

        [HttpPost("/student-login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyUsername(request.Username);
            if (student == null || student.Password != request.Password)
                return BadRequest("not found username");
            var accessToken = _tokenGenerator.GenerateAccessToken(student);

            return Ok(new { account = student, accessToken = accessToken });
        }

        [HttpGet("student-get-password-in-mail/{username}")]
        public async Task<IActionResult> RecoverPassword(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyUsername(username);
            if (student == null)
                return BadRequest("not found username");
            if (student.Email == null)
                return BadRequest("user's email doesn't exist ");

            var isSent = await _emailUtil.SendEmailAsync(student.Email, "no-reply: your password is", "this is your password" + student.Password);

            return Ok(isSent);
        }


        [HttpGet("/student-get-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var student = await _studentRepository.GetbyId(id);
            return Ok(student);
        }

        [HttpGet("/student-get-many-range/{start}/{end}")]
        public async Task<IActionResult> ManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetManyRange(start, end);
            return Ok(students);
        }
        [HttpPost("/student-get-many-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var students = await _studentRepository.GetManyfromIds(ids);
            return Ok(students);
        }


        [HttpDelete("/student-delete/{id}")]
        public async Task<IActionResult> Delete(string id, [FromBody] string? prevUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var deleteTask = _studentRepository.Delete(id);
            if (!string.IsNullOrWhiteSpace(prevUrl))
                await Task.WhenAll(deleteTask, _cloudinaryHandler.Delete(prevUrl));
            else
                await deleteTask;
            return Ok(true);
        }
        [HttpDelete("/student-delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteManyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ids = request.Ids;
            var prevUrls = request.PrevUrls;

            var filter = Builders<Student>.Filter.In(l => l.ID, ids);
            var deleteTask = _studentCollection.DeleteManyAsync(filter);
            List<Task> deleteImgTasks = new List<Task>();
            if (prevUrls != null && prevUrls.Count > 0)
                foreach (var url in prevUrls)
                    deleteImgTasks.Add(_cloudinaryHandler.Delete(url));

            await Task.WhenAll(deleteTask, Task.WhenAll(deleteImgTasks));
            return Ok(true);
        }

        [HttpPost("/student-update-instance")]
        public async Task<IActionResult> UpdateInstance([FromForm] FormDataRequest formDataRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!TryDeserializeJson(formDataRequest.Requestbody, out SchoolMemberUpdateRequest? request))
                return BadRequest(request);
            if (request == null)
                return BadRequest(ModelState);


            Student student = _mapper.Map<Student>(request);    
            if(formDataRequest.File!=null && formDataRequest.File.Length > 0)
            {
                var fileUrl = await _cloudinaryHandler.UploadSingleImage(formDataRequest.File, _studentFolderName);
                if (fileUrl != null)
                    student.PersonalInfo.AvatarUrl = fileUrl;
            }

            var updateTask = _studentRepository.UpdatebyInstance(student.ID, student);
            if(request.PrevUrl!=null)
                await Task.WhenAll(_cloudinaryHandler.Delete(request.PrevUrl), updateTask);
            else
            await updateTask;

            return Ok(student);
        }

        private bool TryDeserializeJson<T>(string? json, out T? result)
        {
            Console.WriteLine(json);
            if (string.IsNullOrWhiteSpace(json))
            {
                result = default(T?);
                return false;
            }
            try
            {
                // Use System.Text.Json.JsonSerializer for case-insensitive deserialization
                result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    // Ignore case during deserialization
                    PropertyNameCaseInsensitive = true,
                    Converters = { new DateTimeConverter("dd/MM/yyyy") }

                });
                return true;
            }
            catch (JsonException)
            {
                // Handle the exception or log it if necessary
                result = default;
                return false;
            }
        }

    }
}
