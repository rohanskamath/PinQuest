using dotnetcorebackend.Application.DTOs.UserDTOs;

namespace backend.Application.Helpers
{
    public interface ITokenHelper
    {
        public string GenerateJwtToken(UserDTO userData);
        public Guid GenerateRefreshToken();
    }
}
