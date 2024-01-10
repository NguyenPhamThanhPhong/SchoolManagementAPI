using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagementAPI.Models.Entities
{
    public class StudentRegistration
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string semesterId { get; set; }
        public List<string>? classIds { get; set; }
        public StudentRegistration()
        {
            ID = string.Empty;
            Name = string.Empty;
            semesterId= string.Empty;
            classIds = new List<string>();
        }
    }
}
