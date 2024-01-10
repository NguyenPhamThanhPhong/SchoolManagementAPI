using SchoolManagementAPI.Models.Enum;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassRegistrationRequest
    {
        public string ID { get; set; }
        public UpdateOption option { get; set; }
        public string StudentId { get; set; }
        public List<int>? Scores { get;set; } 
        public SchoolClassRegistrationRequest()
        {
            ID = string.Empty;
            StudentId = string.Empty;
            Scores = new List<int> { -1, -1, -1, -1, -1 };
        }

    }
}
