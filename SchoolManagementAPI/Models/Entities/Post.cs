using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagementAPI.Models.Entities
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public string? Title { get; set; }
        public List<string>? FacultyTags { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Content { get; set; }
        public Dictionary<string,string?>? FileUrls { get; set; }
        public Post()
        {
            ID = string.Empty;
            FacultyTags= new List<string>();
            CreatedTime = DateTime.Now;
            Content = string.Empty;
        }

    }
}
