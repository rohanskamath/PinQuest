using BCrypt.Net;
using dotnetcorebackend.Application.Repositories.UserRepository;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, object>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserPasswordHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<object> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser is null)
                {
                    return new { success = false, message = "User not found!.. Kindly register!" };
                }
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                bool result = await _userRepository.ChangePasswordAsync(existingUser);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
