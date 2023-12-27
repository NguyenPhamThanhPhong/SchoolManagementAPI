﻿using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.SchoolClass;
using SchoolManagementAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassCreateRequest
    {
        [Required]
        public string ID { get; set; }
        public string? Name { get; set; }
        public string? RoomName { get; set; }
        public string? Program { get; set; }
        public string? ClassType { get; set; }
        [Required]
        public string? SubjectId { get; set; }
        public string? SemesterId { get; set; }
        public ClassSchedule? Schedule { get; set; }
        public List<StudentLog>? StudentLog { get; set; }
    }
}
