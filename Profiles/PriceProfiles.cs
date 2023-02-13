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
        }
    }
}