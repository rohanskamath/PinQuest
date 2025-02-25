using AutoMapper;
using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Helpers;
using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Services.test.UserService.test
{
    public class LoginUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<TokenCreationHelper> _mockTokenCreationHelper;
        private readonly LoginUserHandler _handler;

        public LoginUserHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            var mockConfiguration = new Mock<IConfiguration>();
            _mockTokenCreationHelper = new Mock<TokenCreationHelper>(mockConfiguration.Object) { CallBase = true };
            _handler = new LoginUserHandler(_mockUserRepository.Object, _mockMapper.Object, _mockTokenCreationHelper.Object);
        }

        /// <summary>
        /// TC001:- Test case should return failure when user is not found.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailureResult()
        {
            var request = new LoginUserCommand { Email = "test@example.com", Password = "test123" };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(request.Email)).ReturnsAsync((User?)null);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            var resultType = result.GetType();
            Assert.False((bool)resultType.GetProperty("success").GetValue(result));
            Assert.Equal("User not found!.. Kindly register!", (string)resultType.GetProperty("message").GetValue(result));

        }

        /// <summary>
        /// TC002:- Test case should return failure when password is invalid
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailedResult()
        {
            var user = new User { Email = "test@example.com", Username = "Testuser", FullName = "Test User", Password = BCrypt.Net.BCrypt.HashPassword("test123") };
            var request = new LoginUserCommand { Email = "test@example.com", Password = "wrongpassword" };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(request.Email)).ReturnsAsync(user);

            var result=await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            var resultType = result.GetType();
            Assert.False((bool)resultType.GetProperty("success").GetValue(result));
            Assert.Equal("Invalid Username/Password!", (string)resultType.GetProperty("message").GetValue(result));
        }

        /// <summary>
        /// TC003:- Test case should return success when credentials are valid
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ValidCredentials_ReturnsSuccessResult()
        {
            var user = new User
            {
                Email = "test@example.com",
                Username = "Testuser",
                FullName = "Test User",
                Password = BCrypt.Net.BCrypt.HashPassword("test123")
            };
            var userDto = new UserDTO { UserId = Guid.NewGuid(), Email = user.Email, Username="Testuser", FullName = "Test User" };
            var request=new LoginUserCommand { Email = "test@example.com", Password = "test123" };
            var fakeToken="abcdefg";
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(request.Email)).ReturnsAsync(user);
            _mockMapper.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDto);
            _mockTokenCreationHelper.Setup(helper=>helper.GenerateJwtToken(userDto)).Returns(fakeToken);

            var result=await _handler.Handle(request, CancellationToken.None);
            _mockUserRepository.Verify(repo=>repo.UpdateUserAsync(It.IsAny<User>()), Times.Exactly(2));

            Assert.NotNull(result);
            var resultType = result.GetType();
            Assert.True((bool)resultType.GetProperty("success").GetValue(result));
            Assert.Equal("LoggedIn successfully!", (string)resultType.GetProperty("message").GetValue(result));
        }
    }
}
