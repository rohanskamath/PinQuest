using dotnetcorebackend.Application.DTOs;
using MediatR;

namespace dotnetcorebackend.Application.UserService.Commands
{
    public class RegisterUserCommand : IRequest<RegisterUserDTO>
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
