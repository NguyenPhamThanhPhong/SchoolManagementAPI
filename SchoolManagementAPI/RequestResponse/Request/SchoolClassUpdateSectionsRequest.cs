namespace SchoolManagementAPI.RequestResponse.Request
{
    public class SchoolClassUpdateSectionsRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile File { get; set; }
    }
}
