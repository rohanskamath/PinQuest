using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using MediatR;

namespace dotnetcorebackend.Application.Services.Pinservice.Queries
{
    public class GetPinsByIdHandler : IRequestHandler<GetPinsByIdQuery, List<GetAllPinsDTO>>
    {
        private readonly IPinsRepository _pinRepository;
        public GetPinsByIdHandler(IPinsRepository pinsRepository)
        {
            _pinRepository = pinsRepository;
        }
        public async Task<List<GetAllPinsDTO>> Handle(GetPinsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _pinRepository.GetPinsByIdAsync(request.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
    }
}
