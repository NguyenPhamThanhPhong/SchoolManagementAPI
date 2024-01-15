using SchoolManagementAPI.Models.Embeded.SchoolClass;

namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassUpdateSectionsRequest
    {
        public List<IFormFile>? FormFiles { get; set; }
        public List<string> PrevUrls { get; set; }
        public List<Section> Sections { get; set; }
        public SchoolClassUpdateSectionsRequest()
        {
            PrevUrls = new List<string>();
            Sections = new List<Section>();
        }
    }
}
