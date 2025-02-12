using dotnetcorebackend.Application.DTOs.UserDTOs;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class RegisterUserCommand : IRequest<UserDTO>
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
