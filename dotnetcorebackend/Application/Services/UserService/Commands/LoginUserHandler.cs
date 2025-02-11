using AutoMapper;
using dotnetcorebackend.Application.DTOs;
using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, object?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        public LoginUserHandler(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<object?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser == null)
                {
                    throw new Exception("User not found!.. Kindly register!");
                }

                if (existingUser.Email != request.Email || !BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
                {
                    throw new Exception("Invalid Username/Password!");
                }

                var userData = _mapper.Map<UserDTO>(existingUser);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, existingUser.Email),
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

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(120),
                    Path = "/"
                };
                _contextAccessor.HttpContext.Response.Cookies.Append("token", tokenString, cookieOptions);
                return new
                {
                    success = true,
                    message = "LoggedIn successfully!",
                    token = tokenString
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
