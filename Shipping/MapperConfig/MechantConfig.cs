using AutoMapper;
using Shipping.DTOs.MerchantDTOs;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class MechantConfig : Profile
    {
        public MechantConfig()
        {
            CreateMap<Merchant, MerchantGetDTO>().ReverseMap();

            //CreateMap<Merchant, MerchantGetDTO>().AfterMap((src, dest) =>
            //{
            //    dest.Mobile1 = src.BranchMobiles?.Br_Mob1;
            //    dest.Mobile2 = src.BranchMobiles?.Br_Mob2;
            //    dest.Phone1 = src.BranchPhones?.Br_Ph1;
            //    dest.Phone2 = src.BranchPhones?.Br_Ph2;
            //    dest.ManagerName = src.Manager?.UserName;
            //}).ReverseMap();

        }
    }
}
