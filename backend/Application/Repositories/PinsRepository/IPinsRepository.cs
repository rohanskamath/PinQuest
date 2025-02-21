using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Models;

namespace dotnetcorebackend.Application.Repositories.PinsRepository
{
    public interface IPinsRepository
    {
        public Task<Pin> CreateNewPinAsync(Pin pin);
        public Task<UserPin> AddUserPinAsync(UserPin userPin);
        public Task<List<GetAllPinsDTO>> GetPinsByIdAsync(Guid userId);
        public Task<List<GetAllPinsDTO>> GetAllPinsAsync();
    }
}
