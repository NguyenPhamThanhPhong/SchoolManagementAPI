using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Enum;

namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class ClassSchedule
    {
        [BsonIgnoreIfNull]
        public string? ID { get; set; }
        [BsonIgnoreIfNull]
        public string? Name { get; set; }
        public DateOfWeek Dateofweek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [BsonIgnoreIfDefault]
        public DateTime BeginTime { get; set; }
        [BsonIgnoreIfDefault]
        public DateTime FinalTime { get; set; }
    }
}
