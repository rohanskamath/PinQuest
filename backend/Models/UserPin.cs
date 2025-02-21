using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcorebackend.Models
{
    public class UserPin
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey("Pin")]
        public Guid PinId { get; set; }
        public Pin? Pin { get; set; }
    }
}
