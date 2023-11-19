using MongoDB.Bson.Serialization.Attributes;
using SchoolManagementAPI.Models.Embeded.ReuseTypes;
#pragma warning disable CS8618
namespace SchoolManagementAPI.Models.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; } = String.Empty;
        public List<Faculty>  Faculties { get; set; }
        public List<Semester> Semesters { get; set; }
        public Category() 
        { 
            Faculties = new List<Faculty>();
            Semesters = new List<Semester>();
        }
    }
}
