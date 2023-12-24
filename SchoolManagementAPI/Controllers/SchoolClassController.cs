﻿using AutoMapper;
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
        public async Task<IActionResult> GetFilter([FromForm]string textFilter)
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
        public async Task<IActionResult> GetfromIds(List<string> ids)
        {
            var classes = await _schoolClassRepository.GetfromIds(ids);
            return Ok(classes);
        }
        [HttpPost("/class-update-schedule/{id}")]
        public async Task<IActionResult> SetSchedule(string id,[FromForm] ClassSchedule schedulePiece)
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
        [HttpPost("/class-update-sections/{id}/{updateOption}")]
        public async Task<IActionResult> UpdateSections(string id, UpdateOption updateOption, [FromForm] List<SchoolClassUpdateSectionsRequest> requests)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (updateOption == UpdateOption.set)
                return BadRequest("can't set, only push and pull");
            List<UpdateParameter> paramters = new List<UpdateParameter>();
            foreach(var request in requests)
            {
                var section = _mapper.Map<Section>(request);
                var paramter = new UpdateParameter
                {
                    fieldName = SchoolClass.GetFieldName(s => s.Schedule),
                    value = section,
                    option = updateOption
                };
                paramters.Add(paramter);
            }
            var isUpdated = await _schoolClassRepository.UpdateByParameters(id, paramters);
            return Ok(isUpdated);
        }
    }
}
