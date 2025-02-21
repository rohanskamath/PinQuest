using dotnetcorebackend.Application.DTOs.UserDTOs;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Queries
{
    public class GetUserByEmailQuery : IRequest<UserDTO>
    {
        public string Email { get; }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
