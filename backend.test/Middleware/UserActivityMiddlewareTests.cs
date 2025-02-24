using dotnetcorebackend.Infrastructure.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Middleware
{
    public class UserActivityMiddlewareTests
    {
        private readonly Mock<ILogger<UserActivityMiddleware>> _loggerMock;
        public UserActivityMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<UserActivityMiddleware>>();
        }

        /// <summary>
        /// Test to check if the middleware logs the action
        /// </summary>
        /// <param name="route"></param>
        /// <param name="email"></param>
        /// <param name="expectedAction"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("/api/register", "rohankamath11@gmail.com", "User Registered")]
        [InlineData("/api/login", "rohankamath11@gmail.com", "User Logged In")]
        public async Task Invoke_validUserRoutes_ShouldLogAction(string route, string email, string expectedAction)
        {
            var middleware = new UserActivityMiddleware(next: (innerHttpContext) => Task.CompletedTask, _loggerMock.Object);
            var context = new DefaultHttpContext();

            context.Request.Path = route;
            context.Request.Method = "POST";
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes($"{{\"email\":\"{email}\"}}"));
            context.Request.ContentLength = context.Request.Body.Length;
            context.Request.Body.Position = 0;

            await middleware.Invoke(context);

            // Check if the Logger received the expected message
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, _) => o.ToString().Contains(expectedAction) && o.ToString().Contains(email)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }

        /// <summary>
        /// Test to check if the middleware does not log the action
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Invoke_NonAuthRoute_ShouldnotLogAction()
        {
            var middleware = new UserActivityMiddleware(next: (innerHttpContext) => Task.CompletedTask, _loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/test";
            context.Request.Method = "POST";

            await middleware.Invoke(context);

            // Logger should not be called
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never
                );
        }

        /// <summary>
        /// Test to check if the middleware does not log the action
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Invoke_LogwithoutEmail_ShouldLogAsAnonymous()
        {
            var middleware = new UserActivityMiddleware(next: (innerHttpContext) => Task.CompletedTask, _loggerMock.Object);
            var context = new DefaultHttpContext();

            context.Request.Path = "/api/register";
            context.Request.Method = "POST";
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));
            context.Request.ContentLength = context.Request.Body.Length;
            context.Request.Body.Position = 0;

            await middleware.Invoke(context);

            // Check if the Logger received the expected message
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, _) => o.ToString().Contains("Anonymous")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }
    }
}
