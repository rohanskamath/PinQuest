using dotnetcorebackend.Application.DTOs.UserDTOs;
using dotnetcorebackend.Application.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace backend.test.Helpers.test
{
    public class TokenCreationHelperTests
    {
        private readonly TokenCreationHelper _tokenCreationHelper;
        private readonly Mock<IConfiguration> _mockIConfiguration;
        public TokenCreationHelperTests()
        {
            _mockIConfiguration = new Mock<IConfiguration>();

            // Mocking IConfiguration using indexer access
            _mockIConfiguration.Setup(x => x["Jwt:Key"]).Returns("supersecretkey12345678901234567890");
            _mockIConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
            _mockIConfiguration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

            _tokenCreationHelper = new TokenCreationHelper(_mockIConfiguration.Object);
        }

        /// <summary>
        /// Tests that the GenerateJwtToken method returns a valid JWT token.
        /// </summary>
        [Fact]
        public void GenerateTokenTest()
        {
            var userDto = new UserDTO { UserId = Guid.NewGuid(), FullName = "Test User", Username = "test", Email = "test@example.com" };

            var token = _tokenCreationHelper.GenerateJwtToken(userDto);

            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.NotNull(jwtToken);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal("TestAudience", jwtToken.Audiences.FirstOrDefault());
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userDto.UserId.ToString());
        }

        /// <summary>
        /// Tests that the GenerateRefreshToken method returns a non-empty Guid.
        /// </summary>
        [Fact]
        public void GenerateRefreshTokenTest()
        {
            var refreshToken = _tokenCreationHelper.GenerateRefreshToken();
            Assert.NotEqual(Guid.Empty, refreshToken);
        }
    }
}
