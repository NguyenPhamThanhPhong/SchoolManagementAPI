using SchoolManagementAPI.Models.Enum;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassRegistrationRequest
    {
        public string ID { get; set; } // class Id
        public string Name { get; set; }
        public UpdateOption option { get; set; }
        public string StudentId { get; set; }

        public SchoolClassRegistrationRequest()
        {
            ID = string.Empty;
            Name = string.Empty;
            StudentId = string.Empty;
        }

    }
}
