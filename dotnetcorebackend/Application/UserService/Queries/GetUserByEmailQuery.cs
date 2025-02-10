using dotnetcorebackend.Application.DTOs;
using MediatR;

namespace dotnetcorebackend.Application.UserService.Queries
{
    public class GetUserByEmailQuery :IRequest<bool>
    {
        public string? Email { get; }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
