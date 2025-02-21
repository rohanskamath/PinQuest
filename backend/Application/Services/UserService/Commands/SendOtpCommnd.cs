using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class SendOtpCommnd : IRequest<object>
    {
        public required string Email { get; set; }
    }
}
