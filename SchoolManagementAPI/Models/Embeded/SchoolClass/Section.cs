using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagementAPI.Models.Embeded.SchoolClass
{

    public class Section
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Dictionary<string,string?>? DocumentUrls { get; set; }
        public Section()
        {
            Title = string.Empty;
            Content = string.Empty;
        }
    }
}
