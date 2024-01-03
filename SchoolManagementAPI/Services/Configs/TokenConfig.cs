namespace SchoolManagementAPI.Services.Configs
{
#pragma warning disable
    public class TokenConfig
    {
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
