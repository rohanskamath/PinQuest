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
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public LoginUserHandler(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<string?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                if (existingUser.Email == request.Email && BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, existingUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserData",JsonSerializer.Serialize(existingUser)),
                    };
                }
                else
                {
                    return "Invalid Username/password";
                }
            }
            return "aa";
        }

        //private string Generate
    }
}
