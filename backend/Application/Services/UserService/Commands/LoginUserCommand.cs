using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class LoginUserCommand :IRequest<object?>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
