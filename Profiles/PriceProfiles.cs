using AutoMapper;
using PriceService.DTO;
using PriceService.Model;

namespace PriceService.Profiles
{
    public class PriceProfiles : Profile
    {
        public PriceProfiles()
        {
            CreateMap<Price, PriceDto>();
            CreateMap<CreatePriceDto, Price>();
            CreateMap<CreatePlatformDto, Platform>()
                .ForMember(
                    dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name));
        }
    }
}