using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.Subject;

namespace SchoolManagementAPI.Models.Entities
{
    public class Subject
    {
        [BsonId]
        public string ID { get; set; } = "";
        public string? Name { get; set; }
        public string? Prequisite { get; set; }
        public string? PreviousSubject { get; set; }
        public ClassLog? Classes { get; set; }
    }
}
