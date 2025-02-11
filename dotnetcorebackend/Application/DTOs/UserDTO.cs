using System.ComponentModel.DataAnnotations;

namespace dotnetcorebackend.Application.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Username { get; set; }
        public Guid UniqueUserTokenId { get; set; }
    }
}