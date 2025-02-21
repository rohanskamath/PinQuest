using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class VerifyOtpCommand : IRequest<object>
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}
