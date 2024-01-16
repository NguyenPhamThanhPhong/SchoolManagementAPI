using SchoolManagementAPI.Models.Embeded.SchoolClass;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassUpdateSectionsRequest
    {
        public List<IFormFile>? FormFiles { get; set; }
        public Dictionary<string,string?>? PrevUrls { get; set; }
        public List<Section> Sections { get; set; }
        public SchoolClassUpdateSectionsRequest()
        {
            PrevUrls = new Dictionary<string, string?>();
            Sections = new List<Section>();
        }
    }
}
