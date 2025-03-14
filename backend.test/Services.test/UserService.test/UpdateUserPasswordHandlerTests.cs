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
    public class UpdateUserPasswordHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UpdateUserPasswordHandler _updateUserPasswordHandler;
        private readonly UpdateUserPasswordCommand _updateUserPasswordCommand;
        public UpdateUserPasswordHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _updateUserPasswordHandler = new UpdateUserPasswordHandler(_mockUserRepository.Object);
            _updateUserPasswordCommand = new UpdateUserPasswordCommand
            {
                Email = "test@example.com",
                NewPassword = "newpassword"
            };
        }

        /// <summary>
        /// TC001:- Test case should update user password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handles_ShouldUpdateUserPassword_WhenValidRequest()
        {
            var mockUser = new User
            {
                Email = "test@example.com",
                FullName = "Test User",
                Username = "TestUser",
                Password = "testpassword"
            };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(mockUser);
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(true);

            var result = await _updateUserPasswordHandler.Handle(_updateUserPasswordCommand, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True((bool)result.GetType().GetProperty("success").GetValue(result));
            Assert.Equal("Password changed sucessfully!", result.GetType().GetProperty("message").GetValue(result));
        }

        /// <summary>
        /// TC002:- Test case should return failure when user is not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserDoesNotExist()
        {
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("nonexistent@example.com")).ReturnsAsync((User)null);

            var result = await _updateUserPasswordHandler.Handle(_updateUserPasswordCommand, CancellationToken.None);
            Assert.NotNull(result);
            Assert.False((bool)result.GetType().GetProperty("success").GetValue(result));
            Assert.Equal("User not found!.. Kindly register!", result.GetType().GetProperty("message").GetValue(result));
        }

        /// <summary>
        /// TC003:- Test case should throw exception when repository throws exception
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await Assert.ThrowsAsync<Exception>(() => _updateUserPasswordHandler.Handle(_updateUserPasswordCommand, CancellationToken.None));
            Assert.Contains("An error occurred while processing your request", result.Message);
        }
    }
}
