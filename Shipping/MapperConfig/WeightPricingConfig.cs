using AutoMapper;
using Shipping.DTOs;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class WeightPricingConfig : Profile
    {
        public WeightPricingConfig()
        {
            CreateMap<WeightPricing, WeightPricingDTO>().AfterMap((src, dest) =>
            {
                dest.DefaultWeight = src.DefaultWeight;
                dest.AdditionalKgPrice = src.AdditionalKgPrice;
            }).ReverseMap();

            CreateMap<WeightPricingDTO, WeightPricing>().AfterMap((src, dest) =>
            {
                dest.DefaultWeight = src.DefaultWeight;
                dest.AdditionalKgPrice = src.AdditionalKgPrice;
            }).ReverseMap();

        }
    }
    

    
}
