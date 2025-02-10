using System.ComponentModel.DataAnnotations;

namespace dotnetcorebackend.Application.DTOs
{
    public class RegisterUserDTO
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Username { get; set; }
    }
}
