namespace SchoolManagementAPI.RequestResponse.Request
{
#pragma warning disable
    public class AccountCreateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "admin";
    }
}
