using AutoMapper;
using Shipping.DTOs.CityDTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class CityConfig : Profile
    {
        public CityConfig()
        {
            CreateMap<City, CityGetDTO>().AfterMap((src, dest) =>
            {
                dest.GovernmentName = src.Government.Name;
            }).ReverseMap();
        }
    }
}
