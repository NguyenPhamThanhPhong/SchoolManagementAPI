namespace SchoolManagementAPI.RequestResponse.Request
{
    public class DeleteManyRequest
    {
        public List<string>? Ids { get; set; }
        public List<string>? PrevUrls { get; set; }
    }
}
