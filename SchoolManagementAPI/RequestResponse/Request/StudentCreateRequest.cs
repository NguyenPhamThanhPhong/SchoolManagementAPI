using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class StudentCreateRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        [Required]
        public PersonalInformation? PersonalInfo { get; set; }
        public List<string>? Classes { get; set; }
        public Dictionary<string, ScheduleAggregation>? ScheduleAggregations { get; set; }
    }
}
