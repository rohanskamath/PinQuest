using MediatR;

namespace dotnetcorebackend.Application.Services.EmailService
{
    public class SendOtpCommnd : IRequest<object>
    {
        public required string Email { get; set; }
    }
}
