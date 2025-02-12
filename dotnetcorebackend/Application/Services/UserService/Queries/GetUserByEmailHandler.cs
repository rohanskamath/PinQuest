using dotnetcorebackend.Application.DTOs.UserDTOs;
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
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser == null)
                {
                    return null;
                }
                return new UserDTO
                {
                    FullName = existingUser.FullName,
                    Email = existingUser.Email,
                    Username = existingUser.Username,
                    UniqueUserTokenId = existingUser.UniqueUserTokenId,
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
