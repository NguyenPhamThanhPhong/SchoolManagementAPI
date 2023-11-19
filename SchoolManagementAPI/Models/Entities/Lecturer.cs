using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.Account;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;

#pragma warning disable CS8618

namespace SchoolManagementAPI.Models.Entities
{
    public class Lecturer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public AccountInformation AccountInfo { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public List<DataLink> Classes { get; set; }
        public List<ScheduleAggregationLink> ScheduleAggregations { get; set; }
        public Lecturer()
        {
            AccountInfo = new AccountInformation();
            PersonalInfo= new PersonalInformation();
            Classes = new List<DataLink>();
            ScheduleAggregations = new List<ScheduleAggregationLink>();
        }

    }
}
