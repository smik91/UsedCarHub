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
            CreateMap<UserEntity, InfoUserDto>().ReverseMap();
            CreateMap<CarEntity, CarDto>().ReverseMap();
            CreateMap<AddAdvertisementDto, AdvertisementEntity>()
                .ForMember(dest => dest.Car, opt => opt.Ignore());
            CreateMap<AdvertisementEntity, AdvertisementDto>().ReverseMap();
            CreateMap<ProfileEntity, ProfileDto>().ReverseMap();
            CreateMap<AdvertisementEntity, UpdateAdvertisementDto>().ReverseMap();
            CreateMap<AdvertisementEntity, InfoAdvertisementDto>()
                .ForMember(dest => dest.Car, opt => opt.Ignore());
        }
    }
}