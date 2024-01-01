using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Enum;

#pragma warning disable CS8618

namespace SchoolManagementAPI.Models.Embeded.Account
{
    public class CreditLog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<int> Scores { get; set; }
        public StudyStatus Status { get; set; }
        public CreditLog() 
        { 
            Scores = new List<int>();
        }
    }
}
