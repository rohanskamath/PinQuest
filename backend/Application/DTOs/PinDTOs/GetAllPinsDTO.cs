namespace dotnetcorebackend.Application.DTOs.PinDTOs
{
    public class GetAllPinsDTO
    {
        public Guid PinId { get; set; }
        public required string Title { get; set; }
        public required string Category { get; set; }
        public required string Description { get; set; }
        public int Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
