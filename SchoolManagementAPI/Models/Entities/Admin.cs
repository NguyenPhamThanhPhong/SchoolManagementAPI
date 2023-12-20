using CloudinaryDotNet;
using MongoDB.Bson.Serialization.Attributes;


namespace SchoolManagementAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Admin : Models.Abstracts.Account
    {
        public Admin() : base()
        {
            this.Role = "admin";
        }
    }
}
