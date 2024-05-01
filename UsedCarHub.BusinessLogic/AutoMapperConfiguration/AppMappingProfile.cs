using AutoMapper;
using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.BusinessLogic.AutoMapperConfiguration
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserEntity, RegisterUserDto>().ReverseMap();
        }
    }
}