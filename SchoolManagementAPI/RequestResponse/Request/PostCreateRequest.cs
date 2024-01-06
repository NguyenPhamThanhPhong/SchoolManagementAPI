namespace SchoolManagementAPI.RequestResponse.Request
{
    public class PostCreateRequest
    {
#pragma warning disable
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> FacultyTags { get; set; }
    }
}
