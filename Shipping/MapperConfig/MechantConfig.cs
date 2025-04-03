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
                dest.CreatedDate = (src.ApplicationUser.CreatedDate)?.ToString("dd MMM yyyy");
                dest.BranchsNames = string.Join(Environment.NewLine, src.BranchMerchants.Select(bm => bm.Branch?.Name ?? "Unknown")); // Get Branch Names
            }).ReverseMap();

            CreateMap<MerchantCreateDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                if (dest.Merchant == null) dest.Merchant = new Merchant();

                dest.Merchant.AppUser_Id = dest.Id;
                dest.Merchant.StoreName = src.StoreName;
                dest.Merchant.Government = src.Government;
                dest.Merchant.City = src.City;
                dest.Merchant.PickupCost = src.PickupCost;
                dest.Merchant.RejectedOrderPercentage = src.RejectedOrderPercentage;

                dest.Merchant.BranchMerchants = src.Branches_Id?
                .Select(branchId => new BranchMerchant
                {
                    Branch_Id = branchId
                }).ToList() ?? new List<BranchMerchant>();

                dest.Merchant.SpecialShippingRates = src.SpecialShippingRates?
                .Select(s => new SpecialShippingRate
                {
                    City_Id = s.City_Id,
                    SpecialPrice = s.SpecialPrice
                }).ToList() ?? new List<SpecialShippingRate>();

                dest.UserName = src.Name;
                dest.Email = src.Email;
                dest.PhoneNumber = src.Phone;
                dest.Address = src.Address;
            }).ReverseMap();
        }
    }
}
