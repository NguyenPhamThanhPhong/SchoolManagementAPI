using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementAPI.Models.Entities
{
    public class Faculty
    {
        [BsonId]
        public string ID { get; set; } = String.Empty;
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
