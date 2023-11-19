using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
using SchoolManagementAPI.Models.Embeded.SchoolClass;

namespace SchoolManagementAPI.Models.Entities
{
    public class SchoolClass
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public string? Name { get; set; }
        public string? RoomName { get; set; }
        public string? Program { get; set; }
        public string? ClassType { get; set; }
        public Subject? Subject { get; set; }
        public Semester? Semester { get; set; }
        public DataLink? Lecturer { get; set; }
        public ClassSchedule? Schedule { get; set; }
        public List<StudentLog> StudentLogs { get; set; }
        public List<ExamMileStone> Exam { get; set; }
        public List<Section> Sections { get; set; }

        public SchoolClass()
        {
            ID = String.Empty;
            StudentLogs = new List<StudentLog>();
            Exam = new List<ExamMileStone>();
            Sections = new List<Section>();
        }
    }
}
