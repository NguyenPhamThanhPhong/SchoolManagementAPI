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

        public SchoolClassController(ISchoolClassRepository schoolClassRepository, IMapper mapper,
            CloudinaryHandler cloudinaryHandler, CloudinaryConfig cloudinaryConfig)
        {
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
            _cloudinaryHandler = cloudinaryHandler;
            _schoolClassFolderName = cloudinaryConfig.ClassSectionFolderName;
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
        [HttpPost("/class-update-instance")]
        public async Task<IActionResult> UpdateInstance([FromBody] SchoolClass schoolClass)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _schoolClassRepository.UpdatebyInstance(schoolClass);
            return Ok(schoolClass);
        }
        [HttpPost("/class-student-registration/{id}/{action}")]
        public async Task<IActionResult> UpdateStudentRegistration(string id, UpdateOption option, [FromBody] StudentLog studentLog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var parameter = new UpdateParameter
            {
                fieldName = SchoolClass.GetFieldName(s => s.StudentLogs),
                value = studentLog,
                option = option
            };
            var isUpdated = await _schoolClassRepository.UpdateByParameters(id, new List<UpdateParameter> { parameter });
            return Ok("updated");
        }

        [HttpDelete("/class-delete/{id}")]
        public async Task<IActionResult> Delete(string id, [FromBody] List<string> prevUrls)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var deleteResult = await _schoolClassRepository.Delete(id);

            if (prevUrls != null)
                foreach (var url in prevUrls)
                    await _cloudinaryHandler.Delete(url);

            if (deleteResult)
                return Ok($"deleted {deleteResult}");
            return BadRequest(deleteResult);
        }
        [HttpGet("/class-get-by-id/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var schoolclass = await _schoolClassRepository.GetSingle(id);
            if (schoolclass != null)
                return Ok(schoolclass);
            return Ok(schoolclass);
        }
        [HttpPost("/class-get-by-filter")]
        public async Task<IActionResult> GetFilter([FromForm] string textFilter)
        {
            var schoolClasses = await _schoolClassRepository.GetbyTextFilter(textFilter);
            return Ok(schoolClasses);
        }
        [HttpGet("/class-get-many-range/{start}/{end}")]
        public async Task<IActionResult> GetManyRange(int start, int end)
        {
            var classes = await _schoolClassRepository.GetManyRange(start, end);
            return Ok(classes);
        }
        [HttpPost("/class-get-many-from-ids/")]
        public async Task<IActionResult> GetfromIds(List<string> ids)
        {
            var classes = await _schoolClassRepository.GetfromIds(ids);
            return Ok(classes);
        }
        [HttpPost("/class-update-schedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromForm] ClassSchedule schedulePiece)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var paramter = new UpdateParameter
            {
                fieldName = SchoolClass.GetFieldName(s => s.Schedule),
                value = schedulePiece,
                option = UpdateOption.set
            };
            var isUpdated = await _schoolClassRepository.UpdateByParameters(id, new List<UpdateParameter> { paramter });


            return Ok(isUpdated);
        }
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
