using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.SchoolClass;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolMemberUpdateRequest
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> Classes { get; set; }
        public string? PrevUrl { get; set; }

        public SchoolMemberUpdateRequest()
        {
            ID = string.Empty;
            Classes = new List<string>();
            PersonalInfo = new PersonalInformation();
        }
    }
}
