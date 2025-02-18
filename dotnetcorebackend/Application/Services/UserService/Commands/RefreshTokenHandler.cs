using AutoMapper;
using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Helpers;
using dotnetcorebackend.Application.Repositories.UserRepository;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly TokenCreationHelper _tokenCreationHelper;

        public RefreshTokenHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, TokenCreationHelper tokenCreationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _tokenCreationHelper = tokenCreationHelper;
        }
        public async Task<object> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Decode th token to get the email
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var validatioParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(request.AccessToken, validatioParameters, out var validatedToken);

                var tokenData = principal.Claims.FirstOrDefault(c => c.Type == "UserData")?.Value;
                var userData = JsonConvert.DeserializeObject<UserDTO>(tokenData);

                var existingUser = await _userRepository.GetByEmailAsync(userData.Email);
                if (existingUser != null || existingUser.RefreshToken.ToString() == userData.RefreshToken.ToString())
                {
                    if (existingUser.RefreshTokenExpiry < DateTime.UtcNow)
                    {
                        var newRefreshToken = _tokenCreationHelper.GenerateRefreshToken();
                        existingUser.RefreshToken = newRefreshToken;
                        existingUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(5);
                        await _userRepository.UpdateUserAsync(existingUser);
                    }

                    // Generate a new JWT Token, when expired using same refresh token
                    var userDTO = _mapper.Map<UserDTO>(existingUser);
                    var newTokenString = _tokenCreationHelper.GenerateJwtToken(userDTO);
                    return new { success = true, token=newTokenString };
                }
                return new { success = false };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
