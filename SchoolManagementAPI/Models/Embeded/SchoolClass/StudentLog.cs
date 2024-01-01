using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{
#pragma warning disable CS8618
    [BsonIgnoreExtraElements]
    public class StudentLog
    {
        public string ID { get; set; }
        public List<int> Scores { get; set; }
        public StudentLog()
        {
            Scores = new List<int>();
        }
    }

    public class RollCall
    {
        public DateTime Time { get; set; }
        public bool IsPresent { get; set; }
    }
}
