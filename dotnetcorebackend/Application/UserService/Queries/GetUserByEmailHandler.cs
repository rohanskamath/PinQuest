using dotnetcorebackend.Application.DTOs;
using dotnetcorebackend.Infrastructure.Repositories.UserRepository;
using MediatR;

namespace dotnetcorebackend.Application.UserService.Queries
{
    public class GetUserByEmailHandler :IRequestHandler<GetUserByEmailQuery, bool>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByEmailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            return existingUser != null;
        }
    }
}
