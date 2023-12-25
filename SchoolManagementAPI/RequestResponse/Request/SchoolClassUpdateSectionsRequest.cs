namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassUpdateSectionsRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<IFormFile>? FormFiles { get; set; }
        public List<string> PrevUrls { get; set; }
        public SchoolClassUpdateSectionsRequest()
        {
            Title = string.Empty;
            Content = string.Empty;
            PrevUrls = new List<string>();
        }
    }
}
