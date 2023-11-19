using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.SchoolClass;

namespace SchoolManagementAPI.Models.Entities
{
    public class ScheduleAggregation
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public string Name { get; set; }
        public List<ClassSchedule>  SchedulePartitions { get; set; }

        public ScheduleAggregation()
        {
            ID = String.Empty;
            Name = String.Empty;
            SchedulePartitions = new List<ClassSchedule>();
        }

    }
}
