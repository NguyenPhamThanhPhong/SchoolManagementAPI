namespace SchoolManagementAPI.RequestResponse.Request
{
#pragma warning disable CS8618
    public class PostUpdateRequest
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> FacultyTags { get; set; }
        public Dictionary<string,string?>? KeepUrls { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
