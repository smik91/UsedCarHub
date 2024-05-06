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
            CreateMap<UserEntity, UpdateUserDto>().ReverseMap();
            CreateMap<UserEntity, UserInfoDto>().ReverseMap();
            CreateMap<CarEntity, CarDto>().ReverseMap();
            CreateMap<AddAdvertisementDto, AdvertisementEntity>()
                .ForMember(dest => dest.Car, opt => opt.Ignore());
            CreateMap<AdvertisementEntity, AdvertisementDto>();
        }
    }
}