using AutoMapper;
using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Services.test.UserService.test
{
    public class RegisterUserHandleTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RegisterUserHandler _registerUserHandler;
        private readonly RegisterUserCommand _registerUserCommand;
        public RegisterUserHandleTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _registerUserHandler = new RegisterUserHandler(_mockUserRepository.Object, _mockMapper.Object);
            _registerUserCommand = new RegisterUserCommand
            {
                Email = "test@example.com",
                FullName = "Test Example",
                UserName = "TestUser",
                Password = "test@123"
            };
        }

        /// <summary>
        /// TC001:- Test case should check for valid request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handles_ShouldRegisterUser_WhenValidRequest()
        {
            var hashedPassword=BCrypt.Net.BCrypt.HashPassword(_registerUserCommand.Password);
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = _registerUserCommand.Email,
                FullName = _registerUserCommand.FullName,
                Username = _registerUserCommand.UserName,
                Password = hashedPassword
            };

            var userDTO = new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                Username = user.Username
            };

            _mockUserRepository.Setup(r=>r.RegisterUserAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(userDTO);

            var result = await _registerUserHandler.Handle(_registerUserCommand, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(userDTO.UserId, result.UserId);
            Assert.Equal(userDTO.Email, result.Email);
            Assert.Equal(userDTO.FullName, result.FullName);
            Assert.Equal(userDTO.Username, result.Username);

            _mockUserRepository.Verify(repo => repo.RegisterUserAsync(It.IsAny<User>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<UserDTO>(It.IsAny<User>()), Times.Once);
        }

        /// <summary>
        /// TC002:- Test case should throw exception when invalid request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handles_ShouldThrowException_WhenInvalidRequest()
        {
            _mockUserRepository.Setup(r=>r.RegisterUserAsync(It.IsAny<User>())).ThrowsAsync(new Exception("User registration failed"));

            await Assert.ThrowsAsync<Exception>(async () => await _registerUserHandler.Handle(_registerUserCommand, CancellationToken.None));
            _mockUserRepository.Verify(repo => repo.RegisterUserAsync(It.IsAny<User>()), Times.Once);
        }

        /// <summary>
        /// TC003:- Test case should return failure when an unexpected error occurs.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsFailure()
        {
            _mockUserRepository.Setup(repo => repo.RegisterUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Database error occurred"));

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _registerUserHandler.Handle(_registerUserCommand, CancellationToken.None);
            });
            Assert.NotNull(exception);
            Assert.Contains("An error occurred while processing your request", exception.Message);
        }
    }
}
