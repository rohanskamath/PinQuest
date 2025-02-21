using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class UpdateUserPasswordCommand: IRequest<object>
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
}
