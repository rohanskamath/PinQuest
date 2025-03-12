using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Application.Services.UserService.Queries;
using dotnetcorebackend.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace backend.test.Controllers.test
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly UserController _userController;
        private readonly RegisterUserCommand _registerUserCommand;
        private readonly LoginUserCommand _loginUserCommand;
        private readonly UpdateUserPasswordCommand _updateUserPasswordCommand;
        private readonly SendOtpCommnd _sendOtpCommnd;
        private readonly VerifyOtpCommand _verifyOtpCommand;
        private readonly RefreshTokenCommand _refreshTokenCommand;

        public UserControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _userController = new UserController(_mockMediator.Object);
            _registerUserCommand = new RegisterUserCommand
            {
                Email = "test@example.com",
                Password = "text123",
                UserName = "testuser",
                FullName = "Test User"
            };
            _loginUserCommand = new LoginUserCommand
            {
                Email = "test@example.com",
                Password = "text123"
            };
            _updateUserPasswordCommand = new UpdateUserPasswordCommand
            {
                Email = "test@example.com",
                NewPassword = "text1234"
            };
            _sendOtpCommnd = new SendOtpCommnd
            {
                Email = "test@example.com"
            };
            _verifyOtpCommand = new VerifyOtpCommand
            {
                Email = "test@example.com",
                Otp = "123456"
            };
            _refreshTokenCommand = new RefreshTokenCommand
            {
                AccessToken = "ValidToken"
            };
        }

        /// <summary>
        /// TC001:- Test to check if email already exists and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Register_EmailAlreadyExists_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserDTO { Email = "test@example.com", Username = "testuser", FullName = "Test User" });

            var result = await _userController.Register(_registerUserCommand) as BadRequestObjectResult;
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
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserDTO)null);

            var result = await _userController.Register(_registerUserCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        /// <summary>
        /// TC003:- Test to check if request is not processing and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Register_SomethingWentWrong_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _userController.Register(_registerUserCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC004:- Test to check login and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Login_Successful_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserDTO { Email = "test@example.com", Username = "testuser", FullName = "Test User" });

            var result = await _userController.Login(_loginUserCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
        /// <summary>
        /// TC005:- Test to check if request is not processing and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Login_SomethingWentWrong_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _userController.Login(_loginUserCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC006:- Test to check invalid credentials
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Login_InvalidCredentials()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new { success = false, message = "Invalid Username/Password!" });

            var result = await _userController.Login(_loginUserCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responsObject = result.Value as dynamic;
            Assert.NotNull(responsObject);
            Assert.False(responsObject.success);
            Assert.Equal("Invalid Username/Password!", responsObject.message);
        }

        /// <summary>
        /// TC007:- Test to check user not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Login_UserNotFound()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new { success = false, message = "User not found!.. Kindly register!" });

            var result = await _userController.Login(_loginUserCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responsObject = result.Value as dynamic;
            Assert.NotNull(responsObject);
            Assert.False(responsObject.success);
            Assert.Equal("User not found!.. Kindly register!", responsObject.message);
        }

        /// <summary>
        /// TC008:- Test to check forgot password and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ForgotPassword_Successful_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(_updateUserPasswordCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = true, message = "Password changed sucessfully!" });

            var result = await _userController.ForgotPassword(_updateUserPasswordCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = result.Value as dynamic;
            Assert.True(responseObject.success);
            Assert.Equal("Password changed sucessfully!", responseObject.message);
        }

        /// <summary>
        /// Tc009:- Test to check user not found in ForgotPassword
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ForgotPassword_UserNotFound_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(_updateUserPasswordCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = false, message = "User not found!.. Kindly register!" });

            var result = await _userController.ForgotPassword(_updateUserPasswordCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = result.Value as dynamic;
            Assert.False(responseObject.success);
            Assert.Equal("User not found!.. Kindly register!", responseObject.message);
        }

        /// <summary>
        /// TC010:- Test to check if request is not processing and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ForgotPassword_SomethingWentWrong_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(_updateUserPasswordCommand, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _userController.ForgotPassword(_updateUserPasswordCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC011:- Test to check SendOTP and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendOTP_ValidRequest_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(_sendOtpCommnd, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = true, message = "OTP sent successfully." });

            var result = await _userController.SendOTP(_sendOtpCommnd) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = result.Value as dynamic;
            Assert.True(responseObject.success);
            Assert.Equal("OTP sent successfully.", responseObject.message);
        }

        /// <summary>
        /// TC012:- Test to check if request is not processing and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendOTP_SomethingWentWrong_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(_sendOtpCommnd, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _userController.SendOTP(_sendOtpCommnd) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC013:- Test to check user not found in SendOTP
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendOTP_UserNotFound_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(_sendOtpCommnd, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = false, message = "User not found!.. Kindly register!" });

            var result = await _userController.SendOTP(_sendOtpCommnd) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = result.Value as dynamic;
            Assert.False(responseObject.success);
            Assert.Equal("User not found!.. Kindly register!", responseObject.message);
        }

        /// <summary>
        /// TC014:- Test to check VerifyOTP and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task VerifyOTP_Successful_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(_verifyOtpCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = true, message = "OTP verified successfully." });

            var result = await _userController.VerifyOTP(_verifyOtpCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseObject = result.Value as dynamic;
            Assert.True(responseObject.success);
            Assert.Equal("OTP verified successfully.", responseObject.message);
        }

        /// <summary>
        /// TC015:- Test to check if request is not processing and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task VerifyOTP_SomethingWentWrong_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(_verifyOtpCommand, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _userController.VerifyOTP(_verifyOtpCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC016:- Test to check invalid OTP
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task VerifyOTP_InvalidOTP_ReturnsFailureResponse()
        {
            _mockMediator.Setup(m => m.Send(_verifyOtpCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = false, message = "Invalid OTP!" });

            var result = await _userController.VerifyOTP(_verifyOtpCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var resultObject = result.Value as dynamic;
            Assert.False(resultObject.success);
            Assert.Equal("Invalid OTP!", resultObject.message);
        }

        /// <summary>
        /// TC017:- Test to check RefreshToken and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RefreshToken_ValidToken_ReturnsOk()
        {
            _mockMediator.Setup(m=>m.Send(_refreshTokenCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = true, token = "newAccessToken" });

            var result=await _userController.RefreshToken(_refreshTokenCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var resultObject = result.Value as dynamic;
            Assert.True(resultObject.success);
        }

        /// <summary>
        /// TC018:- Test to check for Invalid token
        /// </summary>
        /// <returns></returns>
        public async Task RefreshToken_InvalidToken_ReturnsFailureResponse()
        {
            _mockMediator.Setup(m => m.Send(_refreshTokenCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new { success = false });

            var result = await _userController.RefreshToken(_refreshTokenCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var responseObject= result.Value as dynamic;
            Assert.False(responseObject.success);
        }

        /// <summary>
        /// TC019:- Test to check if request is not processing and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RefreshToken_SomethingWentWrong_ReturnsBadRequest()
        {
            _mockMediator.Setup(m=>m.Send(_refreshTokenCommand, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result=await _userController.RefreshToken(_refreshTokenCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }
    }
}
