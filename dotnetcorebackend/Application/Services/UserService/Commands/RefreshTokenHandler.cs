using dotnetcorebackend.Application.Repositories.UserRepository;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public RefreshTokenHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<object> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userData = await _userRepository.GetByEmailAsync(request.Email);
                if (userData == null || userData.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    return new { success = false, message = "Invalid/Expired Token" };
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
