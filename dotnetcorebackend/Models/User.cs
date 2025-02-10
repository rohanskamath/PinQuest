using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace dotnetcorebackend.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
        public Guid UniqueUserTokenId { get; set; }

        // Navigation Property
        public ICollection<UserPin> UserPins { get; set; } = new List<UserPin>();

    }
}