using AutoMapper;
using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Models;

namespace dotnetcorebackend.Infrastructure.Mappings
{
    public class PinProfile:Profile
    {
        public PinProfile()
        {
            CreateMap<PinDTO, Pin>().ReverseMap();            
        }
    }
}
