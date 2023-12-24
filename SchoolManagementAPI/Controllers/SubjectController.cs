﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("/subject-view/{start}/{end}")]
        public async Task<IActionResult> GetManyRange(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subjects = await _subjectRepository.GetManyRange(start, end);
            return Ok(subjects);
        }
        [HttpGet("/subject-view/{id}")]
        public async Task<IActionResult> GetManyRange(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subject = await _subjectRepository.GetOne(id);
            return Ok(subject);
        }
        [HttpPost("/subject-update-instance")]
        public async Task<IActionResult> UpdateInstance( [FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _subjectRepository.UpdatebyInstance(subject);
            return Ok(subject);
        }
        [HttpDelete("/delete-subject")]
        public async Task<IActionResult> Delete( string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _subjectRepository.DeleteOne(id);
            return Ok("deleted");
        }


    }
}
