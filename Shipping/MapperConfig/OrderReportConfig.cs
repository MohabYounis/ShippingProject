using AutoMapper;
using Shipping.DTOs.OrderReportDTOs;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class OrderReportConfig : Profile
    {
        public OrderReportConfig()
        {
            CreateMap<Order, OrderReportGetDTO>().AfterMap((src, dest) =>
            {
                dest.OrderStatus = src.OrderStatus.ToString();
                dest.MerchantName = src.Merchant.ApplicationUser.UserName;
                dest.ClientName = src.ClientName;
                dest.ClientPhone = $"{src.ClientPhone1}\n{src.ClientPhone2}";
                dest.Governrate = src.Government.Name;
                dest.City = src.City.Name;
                dest.OrderCost = src.OrderCost;
                dest.ShippingCost = src.ShippingCost;
                dest.CreatedDate = src.CreatedDate.ToString("dd MMM yyyy hh.mmtt");
                dest.DeliveryRight = src.DeliveryRight;
                dest.CompanyRight = src.CompanyRight;
            }).ReverseMap();
        }
    }
}
