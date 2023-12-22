using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolClassController : ControllerBase
    {
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IMapper _mapper;

        public SchoolClassController(ISchoolClassRepository schoolClassRepository, IMapper mapper)
        {
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
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
        [HttpPost("/class-student-registration-action/{id}/{action}")]
        public async Task<IActionResult> UpdateStudentRegistration(string id,UpdateOption option,[FromBody] StudentLog studentLog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var parameter = new UpdateParameter {
                fieldName = SchoolClass.GetFieldName(s => s.StudentLogs),
                value = studentLog,
                option = option
            };
            var isUpdated = await _schoolClassRepository.UpdateByParameters(id,new List<UpdateParameter> { parameter});
            return Ok("updated");
        }

        [HttpDelete("/class-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteResult = await _schoolClassRepository.Delete(id);
            if (deleteResult)
                return Ok($"deleted {deleteResult}");
            return BadRequest(deleteResult);
        }
        [HttpGet("/get-single/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var schoolclass = await _schoolClassRepository.GetSingle(id);
            if(schoolclass!=null)
                return Ok(schoolclass);
            return BadRequest("false");
        }
        [HttpPost("/get-filter")]
        public async Task<IActionResult> GetFilter(string textFilter)
        {
            var schoolClasses = await _schoolClassRepository.GetbyTextFilter(textFilter);
            return Ok(schoolClasses);
        }
        [HttpGet("/class-get-many-range/{start}/{end}")]
        public async Task<IActionResult> GetManyRange(int start,int end)
        {
            var classes = await _schoolClassRepository.GetManyRange(start, end);
            return Ok(classes);
        }
        [HttpPost("/class-get-from-ids/")]
        public async Task<IActionResult> GetManyRange(List<string> ids)
        {
            var classes = await _schoolClassRepository.GetfromIds(ids);
            return Ok(classes);
        }
    }
}
