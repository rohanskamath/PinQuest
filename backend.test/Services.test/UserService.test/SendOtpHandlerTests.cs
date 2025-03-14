using AutoMapper;
using dotnetcorebackend.Application.Repositories.EmailRepository;
using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Models;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Services.test.UserService.test
{
    public class SendOtpHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly IMemoryCache _memoryCache;
        private readonly SendOtpHandler _sendOtpHandler;
        private readonly SendOtpCommnd _sendOtpCommnd;

        public SendOtpHandlerTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockUserRepository = new Mock<IUserRepository>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _sendOtpHandler = new SendOtpHandler(_mockUserRepository.Object, _memoryCache, _mockEmailService.Object);
            _sendOtpCommnd = new SendOtpCommnd
            {
                Email = "test@example.com"
            };
        }

        /// <summary>
        /// TC001:- Test case should send otp to user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handles_ShouldSendOtp_WhenValidRequest()
        {
            var mockuser = new User
            {
                Email = "test@example.com",
                FullName = "Test User",
                Username = "TestUser",
                Password = "testpassword"
            };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(mockuser);
            _mockEmailService.Setup(service => service.SendEmailAsync("test@example.com", It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var result = await _sendOtpHandler.Handle(_sendOtpCommnd, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True((bool)result.GetType().GetProperty("success").GetValue(result));
            Assert.Equal("OTP sent successfully.", result.GetType().GetProperty("message").GetValue(result));
            Assert.True(_memoryCache.TryGetValue("test@example.com", out string cachedOtp));
            Assert.NotNull(cachedOtp);
            Assert.Equal(4, cachedOtp.Length);
        }

        /// <summary>
        /// TC002:- Test case should return failure when user is not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserDoesNotExist()
        {
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("nonexistent@example.com")).Returns(Task.FromResult<User>(null));

            var result = await _sendOtpHandler.Handle(_sendOtpCommnd, CancellationToken.None);
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
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ThrowsAsync(new Exception("Database error"));

            var result = await Assert.ThrowsAsync<Exception>(() => _sendOtpHandler.Handle(_sendOtpCommnd, CancellationToken.None));
            Assert.Contains("An error occurred while processing your request", result.Message);
        }
    }
}
