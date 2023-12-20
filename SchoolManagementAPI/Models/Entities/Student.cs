using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Embeded.Account;

namespace SchoolManagementAPI.Models.Entities
{
    public class Student : SchoolMember
    {
        public CreditLog CreditInfo { get; set; }
        public string? Programs { get; set; }

        public Student(): base()
        {
            Role = "student";
            this.CreditInfo = new CreditLog();

        }

    }
}
