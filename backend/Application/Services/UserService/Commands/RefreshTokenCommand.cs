using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Commands
{
    public class RefreshTokenCommand : IRequest<object>
    {
        public required string AccessToken { get; set; }
    }
}
