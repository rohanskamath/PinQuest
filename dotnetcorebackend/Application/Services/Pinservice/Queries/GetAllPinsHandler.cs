using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using MediatR;

namespace dotnetcorebackend.Application.Services.Pinservice.Queries
{
    public class GetAllPinsHandler : IRequestHandler<GetAllPinsQuery, List<GetAllPinsDTO>>
    {
        private readonly IPinsRepository _pinsRepository;
        public GetAllPinsHandler(IPinsRepository pinsRepository)
        {
            _pinsRepository = pinsRepository;
        }
        public async Task<List<GetAllPinsDTO>> Handle(GetAllPinsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _pinsRepository.GetAllPinsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }

    }
}
