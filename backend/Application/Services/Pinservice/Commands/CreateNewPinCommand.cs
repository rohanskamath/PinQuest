using dotnetcorebackend.Application.DTOs.PinDTOs;
using MediatR;

namespace dotnetcorebackend.Application.Services.Pinservice.Commands
{
    public class CreateNewPinCommand :IRequest<object?>
    {
        public Guid UserId { get; set; }
        public required string Title { get; set; }
        public required string Category { get; set; }
        public required string Description { get; set; }
        public int Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
