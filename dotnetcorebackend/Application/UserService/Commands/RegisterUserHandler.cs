using AutoMapper;
using dotnetcorebackend.Application.DTOs;
using dotnetcorebackend.Infrastructure.Repositories.UserRepository;
using dotnetcorebackend.Models;
using MediatR;

namespace dotnetcorebackend.Application.UserService.Commands
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public RegisterUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<RegisterUserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var hashedPassword=BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Mapping Commad to domain model
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                FullName = request.FullName,
                Username = request.UserName,
                Password = hashedPassword,
            };

            var registeredUser = await _userRepository.RegisterUserAsync(user);

            // Convert User Model to RegisterUserDTO
            var userRegisterDTO = _mapper.Map<RegisterUserDTO>(registeredUser);
            return userRegisterDTO;
        }
    }
}
