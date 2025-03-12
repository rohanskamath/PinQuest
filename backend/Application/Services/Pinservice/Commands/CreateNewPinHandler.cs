using AutoMapper;
using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using dotnetcorebackend.Models;
using MediatR;
using System.Net.NetworkInformation;

namespace dotnetcorebackend.Application.Services.Pinservice.Commands
{
    public class CreateNewPinHandler : IRequestHandler<CreateNewPinCommand, object?>
    {
        private readonly IPinsRepository _pinsRepository;
        private readonly IMapper _mapper;
        public CreateNewPinHandler(IPinsRepository pinsRepository, IMapper mapper)
        {
            _pinsRepository = pinsRepository;
            _mapper = mapper;
        }

        public async Task<object?> Handle(CreateNewPinCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Mapping Command request to Domain model
                var pinData = new Pin
                {
                    PinId = Guid.NewGuid(),
                    Title = request.Title,
                    Category = request.Category,
                    Description = request.Description,
                    Rating = request.Rating,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                };

                var newPin = await _pinsRepository.CreateNewPinAsync(pinData);
                var userPinData = new UserPin
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    PinId = newPin.PinId
                };

                await _pinsRepository.AddUserPinAsync(userPinData);

                // Convert Pin Model to PinDTO
                var pinDTO = _mapper.Map<PinDTO>(newPin);
                return new { success = true, message = "Pins fetched successfully!", data = pinDTO };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
