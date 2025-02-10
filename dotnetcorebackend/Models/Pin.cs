using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcorebackend.Models
{
    public class Pin
    {
        [Key]
        public Guid PinId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Category { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        // Navigation Property
        public ICollection<UserPin> UserPins { get; set; } = new List<UserPin>();

    }
}
