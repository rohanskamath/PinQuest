using AutoMapper;
using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Helpers;
using dotnetcorebackend.Application.Repositories.UserRepository;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenCreationHelper _tokenCreationHelper;

        public RefreshTokenHandler(IUserRepository userRepository, IMapper mapper, TokenCreationHelper tokenCreationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenCreationHelper = tokenCreationHelper;
        }
        public async Task<object> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser == null || existingUser.RefreshToken.ToString() != request.RefreshToken)
                {
                    return new { success = false, message = "Invalid/Expired Token" };
                }
                if (existingUser.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    var newRefreshToken = _tokenCreationHelper.GenerateRefreshToken();
                    existingUser.RefreshToken = newRefreshToken;
                    existingUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(5);
                    await _userRepository.UpdateUserAsync(existingUser);
                }

                var userDTO = _mapper.Map<UserDTO>(existingUser);

                var newTokenString = _tokenCreationHelper.GenerateJwtToken(userDTO);

                return new { success = true, message = "New Token generated sccuessfully!", token = newTokenString };

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
