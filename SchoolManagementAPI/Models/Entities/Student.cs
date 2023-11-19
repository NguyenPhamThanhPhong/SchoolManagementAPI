using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;

namespace SchoolManagementAPI.Models.Entities
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public AccountInformation? AccountInfo { get; set; }
        public PersonalInformation? PersonalInfo { get; set; }
        public List<DataLink>? Classes { get; set; }
        public List<CreditLog>? CreditHistory { get; set; }
        public List<ScheduleAggregationLink>? ScheduleAggregations { get; set; }

    }
}
