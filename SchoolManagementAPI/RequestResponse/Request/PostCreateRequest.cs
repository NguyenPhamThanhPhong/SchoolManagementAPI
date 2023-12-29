namespace SchoolManagementAPI.RequestResponse.Request
{
    public class PostCreateRequest
    {
#pragma warning disable
        public string Titile { get; set; }
        public string Content { get; set; }
        public List<string> FacultyTags { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
