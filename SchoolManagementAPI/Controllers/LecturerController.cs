using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Authentication;
using SchoolManagementAPI.Services.CloudinaryService;
using SchoolManagementAPI.Services.Configs;
using SchoolManagementAPI.Services.SMTP;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IMapper _mapper;
        private readonly EmailUtil _emailUtil;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IMongoCollection<Lecturer> _lecturerCollection;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _lecturerFolderName;


        public LecturerController(ILecturerRepository lecturerRepository, IMapper mapper, EmailUtil emailUtil, 
            ISchoolClassRepository schoolClassRepository, TokenGenerator tokenGenerator, 
            DatabaseConfig databaseConfig, CloudinaryHandler cloudinaryHandler, CloudinaryConfig cloudinaryConfig)
        {
            _lecturerRepository = lecturerRepository;
            _mapper = mapper;
            _emailUtil = emailUtil;
            _schoolClassRepository = schoolClassRepository;
            _tokenGenerator = tokenGenerator;
            _lecturerCollection = databaseConfig.LecturerCollection;
            _cloudinaryHandler = cloudinaryHandler;
            _lecturerFolderName = cloudinaryConfig.LecturerFolderName;
        }
        [HttpPost("/lecturer-create")]
        public async Task<IActionResult> Create([FromBody] SchoolMemberCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturer = _mapper.Map<Lecturer>(request);
            await _lecturerRepository.Create(lecturer);

            return Ok(lecturer);
        }


        [HttpPost("/lecturer-login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturer = await _lecturerRepository.GetbyUsername(request.Username);
            if (lecturer == null || lecturer.Password != request.Password)
                return BadRequest("not found username");
            var accessToken = _tokenGenerator.GenerateAccessToken(lecturer);
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
            });
            return Ok(lecturer);
        }

        [HttpGet("/lecturer-get-many-range/{start}/{end}")]
        public async Task<IActionResult> ManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturers = await _lecturerRepository.GetManyRange(start, end);
            return Ok(lecturers);
        }
        [HttpPost("/lecturer-get-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturers = await _lecturerRepository.GetManyfromIds(ids);
            return Ok(lecturers);
        }


        [HttpGet("lecturer-get-password-in-mail/{username}")]
        public async Task<IActionResult> RecoverPassword(string username)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var lecturer = await _lecturerRepository.GetbyUsername(username);
            if (lecturer == null)
                return BadRequest("not found username");
            if (lecturer.Email == null)
                return BadRequest("user's email doesn't exist ");

            var isSent = await _emailUtil.SendEmailAsync(lecturer.Email, "no-reply: your password is", "this is your password" + lecturer.Password);

            return Ok(isSent);
        }


        [HttpDelete("/lecturer-delete/{id}")]
        public async Task<IActionResult> Delete(string id, [FromBody] string? prevUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var deleteTask = _lecturerRepository.Delete(id);
            if (!string.IsNullOrWhiteSpace(prevUrl))
                await Task.WhenAll(deleteTask, _cloudinaryHandler.Delete(prevUrl));
            else
                await deleteTask;
            return Ok(true);
        }

        [HttpDelete("/lecturer-delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteManyRequest request) // a list of no urls
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var prevUrls = request.PrevUrls;
            var filter = Builders<Lecturer>.Filter.In(l => l.ID, request.Ids);
            var deleteTask = _lecturerCollection.DeleteManyAsync(filter);

            List<Task> deleteImgTasks = new List<Task>();
            if (prevUrls != null && prevUrls.Count > 0)
                foreach (var url in prevUrls)
                    deleteImgTasks.Add(_cloudinaryHandler.Delete(url));

            await Task.WhenAll(deleteTask, Task.WhenAll(deleteImgTasks));
            return Ok(true);
        }

        [HttpPost("/lecturer-update-instance/{prevName}")]
        public async Task<IActionResult> UpdateStringFields([FromForm] FormDataRequest formDataRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!TryDeserializeJson(formDataRequest.Requestbody, out SchoolMemberUpdateRequest? request))
                return BadRequest(request);
            if (request == null)
                return BadRequest(ModelState);

            var lecturer = _mapper.Map<Lecturer>(request);
            if (formDataRequest.File != null && formDataRequest.File.Length > 0)
            {
                var fileUrl = await _cloudinaryHandler.UploadSingleImage(formDataRequest.File, _lecturerFolderName);
                if (fileUrl != null)
                    lecturer.PersonalInfo.AvatarUrl = fileUrl;
            }

            var updateTask = _lecturerRepository.UpdatebyInstance(lecturer);
            if (request.PrevUrl != null)
                await Task.WhenAll(_cloudinaryHandler.Delete(request.PrevUrl), updateTask);
            else
                await updateTask;

            return Ok(lecturer);
        }

        private bool TryDeserializeJson<T>(string? json, out T? result)
        {
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
                    PropertyNameCaseInsensitive = true
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
