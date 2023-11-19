using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Enum;

namespace SchoolManagementAPI.Models.Embeded.Account
{
    public class CreditLog
    {
        public Semester Semester { get; set; }
        public List<Subject> subjects { get; set; }
    }
    public class Subject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StudyStatus Status { get; set; }
    }
}
