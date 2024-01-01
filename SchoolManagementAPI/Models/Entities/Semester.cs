using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementAPI.Models.Entities
{
#pragma warning disable CS8618
    [BsonIgnoreExtraElements]
    public class Semester
    {
        [BsonId]
        [Required]
        public string ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> ClassIds { get; set; }
        public Semester()
        {
            ClassIds = new List<string>();
        }
    }
}
