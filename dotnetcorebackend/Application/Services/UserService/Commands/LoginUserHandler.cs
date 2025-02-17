using AutoMapper;
using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Models;
using dotnetcorebackend.Application.Helpers;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, object?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenCreationHelper _tokenCreationHelper;
        public LoginUserHandler(IUserRepository userRepository, IMapper mapper, TokenCreationHelper tokenCreationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenCreationHelper = tokenCreationHelper;
        }

        public async Task<object?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser == null)
                {
                    return new { success = false, message = "User not found!.. Kindly register!" };
                }

                if (existingUser.Email != request.Email || !BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
                {
                    return new { success = false, message = "Invalid Username/Password!", };
                }

                var refreshToken = _tokenCreationHelper.GenerateRefreshToken();
                existingUser.RefreshToken = refreshToken;
                existingUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(5);

                var userData = _mapper.Map<UserDTO>(existingUser);

                var tokenString = _tokenCreationHelper.GenerateJwtToken(userData);
                _tokenCreationHelper.SetAuthCookie("token", tokenString);

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
