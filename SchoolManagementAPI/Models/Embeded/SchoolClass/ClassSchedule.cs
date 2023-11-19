using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Enum;

namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
    public class ClassSchedule
    {
        [BsonIgnoreIfNull]
        public string ID { get; set; }
        public DateOfWeek Dateofweek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
