using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]

    public class Faculty
    {
        [BsonId]
        public string ID { get; set; } = String.Empty;
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> SubjectIds { get; set; }
        public List<string> PostIds { get; set; }

        public Faculty()
        {
            SubjectIds = new List<string>();
            PostIds = new List<string>();
        }

    }
}
