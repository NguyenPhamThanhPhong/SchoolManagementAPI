using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.Subject;

namespace SchoolManagementAPI.Models.Entities
{
    public class Subject
    {
        public string ID { get; set; } = "";
        public string? Name { get; set; }
        public DataLink?  PrequisiteSubject { get; set; }
        public DataLink? PreviousSubject { get; set; }
        public ClassLog? Classes { get; set; }
    }
}
