using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Entities;

namespace SchoolManagementAPI.Models.Abstracts
{
    public class SchoolMember : Account
    {
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> Classes { get; set; }
        public Dictionary<string,ScheduleAggregation> ScheduleAggregations { get; set; } 
        // string: ID của semester

        public SchoolMember() {
            PersonalInfo = new PersonalInformation();
            Classes = new List<string>();
            ScheduleAggregations = new Dictionary<string,ScheduleAggregation>();
        }
    }
}
