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
                dest.ClientPhone = $"{src.ClientPhone1}-{src.ClientPhone2}";
                dest.Governrate = src.Government.Name;
                dest.City = src.City.Name;
                dest.OrderCost = src.OrderCost;
                dest.ShippingCost = src.ShippingCost;
                dest.CreatedDate = src.CreatedDate.ToString("dd MMM yyyy hh:mm tt");
                dest.DeliveryRight = src.DeliveryRight ?? 0;
                dest.CompanyRight = src.CompanyRight ?? 0;
            }).ReverseMap();
        }
    }
}
