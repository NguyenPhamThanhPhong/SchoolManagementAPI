using Microsoft.IdentityModel.Tokens;
using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Services.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementAPI.Services.Authentication
{
    public class TokenGenerator
    {
        private readonly TokenConfig _tokenConfigs;

        public TokenGenerator(TokenConfig tokenConfigs)
        {
            _tokenConfigs = tokenConfigs;
        }
        public string GenerateAccessToken(Account account)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _tokenConfigs.AccessTokenSecret));
            SigningCredentials credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,account.ID),
                new Claim(ClaimTypes.Email,account.Email??""),
                new Claim(ClaimTypes.Name,account.Username??""),
                new Claim(ClaimTypes.Role,account.Role ?? ""),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _tokenConfigs.Issuer,
                _tokenConfigs.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_tokenConfigs.AccessTokenExpirationMinutes),
                credential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
