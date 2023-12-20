using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace SchoolManagementAPI.RequestResponse.Request
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Email { get; set; }

    }
}
