using AutoMapper;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.NewFolder1;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class MechantConfig : Profile
    {
        public MechantConfig()
        {
            CreateMap<Merchant, MerchantGetDTO>().AfterMap((src, dest) =>
            {
                dest.Name = src.ApplicationUser.UserName;
                dest.Email = src.ApplicationUser.Email;
                dest.Phone = src.ApplicationUser.PhoneNumber;
                dest.Address = src.ApplicationUser.Address;
            }).ReverseMap();

            CreateMap<MerchantCreateDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                if (dest.Merchant == null)
                {
                    dest.Merchant = new Merchant();
                }

                dest.Merchant.StoreName = src.StoreName;
                dest.Merchant.Government = src.Government;
                dest.Merchant.City = src.City;
                dest.Merchant.PickupCost = src.PickupCost;
                dest.Merchant.RejectedOrderPercentage = src.RejectedOrderPercentage;
                dest.UserName = src.Name;
                dest.Email = src.Email;
                dest.PhoneNumber = src.Phone;
                dest.Address = src.Address;
            }).ReverseMap();

        }
    }
}
