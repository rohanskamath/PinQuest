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
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenCreationHelper(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
        public string GenerateAccessToken(UserDTO userData)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub, userData.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim("UserData",JsonSerializer.Serialize(userData)),
            };
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Guid GenerateRefreshToken()
        {
            return Guid.NewGuid();
        }
        public void SetAuthCookie(string cookieName, string tokenString)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(120),
                Path = "/"
            };
            _contextAccessor.HttpContext?.Response.Cookies.Append(cookieName, tokenString, cookieOptions);
        }
    }
}
