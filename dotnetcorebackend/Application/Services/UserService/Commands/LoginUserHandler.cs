using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Models;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string?>
    {
        private readonly IUserRepository _userRepository;
        public LoginUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                if (existingUser.Email == request.Email && BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
                {
                    return "LoggedIn successfully!";
                }
                else
                {
                    return "Invalid Username/password";
                }
            }
            return "aa";
        }
    }
}
