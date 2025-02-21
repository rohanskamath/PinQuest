using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Infrastructure.Context;
using dotnetcorebackend.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcorebackend.Application.Repositories.PinsRepository
{
    public class PinsRepositoryImplementation : IPinsRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public PinsRepositoryImplementation(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<Pin> CreateNewPinAsync(Pin pin)
        {
            await _applicationDBContext.Pins.AddAsync(pin);
            await _applicationDBContext.SaveChangesAsync();
            return pin;
        }

        public async Task<UserPin> AddUserPinAsync(UserPin userPin)
        {
            await _applicationDBContext.UserPins.AddAsync(userPin);
            await _applicationDBContext.SaveChangesAsync();
            return userPin;
        }

        public async Task<List<GetAllPinsDTO>> GetAllPinsAsync()
        {
            var pinsList = await _applicationDBContext.UserPins
                .Include(up => up.User)
                .Include(up => up.Pin)
                .Select(up => new GetAllPinsDTO
                {
                    PinId = up.PinId,
                    Title = up.Pin.Title,
                    Category = up.Pin.Category,
                    Description = up.Pin.Description,
                    Rating = up.Pin.Rating,
                    Latitude = up.Pin.Latitude,
                    Longitude = up.Pin.Longitude,
                    UserId = up.UserId,
                    Username = up.User.Username
                }).ToListAsync();

            return pinsList;
        }

        public async Task<List<GetAllPinsDTO>> GetPinsByIdAsync(Guid userId)
        {
            var pinsList = await _applicationDBContext.UserPins
                .Include(up => up.User)
                .Include(up => up.Pin)
                .Where(up => up.UserId == userId)
                .Select(up => new GetAllPinsDTO
                {
                    PinId = up.PinId,
                    Title = up.Pin.Title,
                    Category = up.Pin.Category,
                    Description = up.Pin.Description,
                    Rating = up.Pin.Rating,
                    Latitude = up.Pin.Latitude,
                    Longitude = up.Pin.Longitude,
                    UserId = up.UserId,
                    Username = up.User.Username
                }).ToListAsync();

            return pinsList;
        }
    }
}
