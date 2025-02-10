using AutoMapper;
using dotnetcorebackend.Application.DTOs;
using dotnetcorebackend.Models;

namespace dotnetcorebackend.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDTO, User>().ReverseMap();
        }
    }
}
