using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;

namespace SchoolManagementAPI.Models.Embeded.Account
{
    [BsonIgnoreExtraElements]
    public class PersonalInformation
    {
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        [BsonIgnoreIfNull]
        public string? Program { get; set; } // CLC hay đại trà
        [BsonIgnoreIfNull]
        public string? Major { get; set; }
        public string? FacultyId { get; set; }
        public string? IDCard { get; set; }
        [BsonIgnoreIfNull]
        public string? JoinYear { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? JoinDate { get; set; }
        [BsonIgnoreIfNull]
        public string? AcademicRank { get; set; }
        [BsonIgnoreIfNull]
        public string? AcademicDegree { get; set; }

    }
}
