using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class RefreshTokenCommand : IRequest<object>
    {
        public required string Email { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
