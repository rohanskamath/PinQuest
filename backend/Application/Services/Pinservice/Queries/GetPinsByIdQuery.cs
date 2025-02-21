using dotnetcorebackend.Application.DTOs.PinDTOs;
using MediatR;

namespace dotnetcorebackend.Application.Services.Pinservice.Queries
{
    public class GetPinsByIdQuery : IRequest<List<GetAllPinsDTO>>
    {
        public Guid UserId { get; set; }
        public GetPinsByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
