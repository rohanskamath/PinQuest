using dotnetcorebackend.Application.DTOs.UserDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace dotnetcorebackend.Application.Helpers
{
    public class TokenCreationHelper
    {
        private readonly IConfiguration _configuration;
        public TokenCreationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(UserDTO userData)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub, userData.UserId.ToString()),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim("UserData",JsonSerializer.Serialize(userData)),
            };
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Guid GenerateRefreshToken()
        {
            return Guid.NewGuid();
        }
    }
}
