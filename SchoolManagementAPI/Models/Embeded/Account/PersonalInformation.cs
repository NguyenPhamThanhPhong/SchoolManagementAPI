using SchoolManagementAPI.Models.Embeded.ReuseTypes;

namespace SchoolManagementAPI.Models.Embeded.Account
{
    public class PersonalInformation
    {
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string?  Faculty { get; set; }
        public string? Program { get; set; } // CLC hay đại trà
    }
}
