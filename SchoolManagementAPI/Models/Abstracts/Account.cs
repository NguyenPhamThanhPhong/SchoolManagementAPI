using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagementAPI.Models.Abstracts
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        public Account()
        {
            this.ID = string.Empty;
        }
    }
}
