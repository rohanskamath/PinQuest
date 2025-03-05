using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Application.Services.UserService.Queries;
using dotnetcorebackend.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Controllers.test
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _userController = new UserController(_mockMediator.Object);
        }

        /// <summary>
        /// TC001:- Test to check if email already exists and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Register_EmailAlreadyExists_ReturnsBadRequest()
        {
            var request = new RegisterUserCommand { Email = "test@example.com", Password = "text123", UserName = "testuser", FullName = "Test User" };
            _mockMediator.Setup(m=>m.Send(It.IsAny<GetUserByEmailQuery>(),It.IsAny<CancellationToken>())).ReturnsAsync(new UserDTO { Email = "test@example.com", Username = "testuser", FullName = "Test User" });

            var result=await _userController.Register(request) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        /// <summary>
        /// TC002:- Test to check register and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Register_Successful_ReturnsOk()
        {
            var request=new RegisterUserCommand { Email = "test@example.com", Password = "text123", UserName = "testuser", FullName = "Test User" };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserDTO)null);

            var result=await _userController.Register(request) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        /// <summary>
        /// TC003:- Test to check login and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Login_Successful_ReturnsOk()
        {
            var request=new LoginUserCommand { Email = "test@example.com", Password = "test123" };
            _mockMediator.Setup(m=>m.Send(It.IsAny<GetUserByEmailQuery>(),It.IsAny<CancellationToken>())).ReturnsAsync(new UserDTO { Email = "test@example.com", Username = "testuser", FullName = "Test User" });

            var result=await _userController.Login(request) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
