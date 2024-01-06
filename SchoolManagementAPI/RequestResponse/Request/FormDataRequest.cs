namespace SchoolManagementAPI.RequestResponse.Request
{
    public class FormDataRequest
    {
        public string? Requestbody { get; set; }
        public List<IFormFile>? Files { get; set; }
        public IFormFile? File { get; set; }
    }
}
