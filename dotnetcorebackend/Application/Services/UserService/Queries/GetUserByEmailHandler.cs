using dotnetcorebackend.Application.DTOs;
using dotnetcorebackend.Application.Repositories.UserRepository;
using MediatR;

namespace dotnetcorebackend.Application.Services.UserService.Queries
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDTO?>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByEmailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            return new UserDTO
            {
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                Username = existingUser.Username,
                UniqueUserTokenId = existingUser.UniqueUserTokenId,
            };
        }
    }
}
