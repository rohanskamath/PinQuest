using dotnetcorebackend.Application.DTOs.PinDTOs;
using MediatR;

namespace dotnetcorebackend.Application.Services.Pinservice.Queries
{
    public class GetAllPinsQuery :IRequest<List<GetAllPinsDTO>>
    {
    }
}
