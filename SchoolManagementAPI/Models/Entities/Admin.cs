using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.Account;


namespace SchoolManagementAPI.Models.Entities
{
    public class Admin
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; } = String.Empty;
        public AccountInformation?  AccountInfo { get; set; }

    }
}
