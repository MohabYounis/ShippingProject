using AutoMapper;
using Shipping.DTOs.OrderDTOs;
using Shipping.Models;
using Shipping.Services.IModelService;

namespace Shipping.MapperConfig
{
    public class OrderConfig : Profile
    {
        public OrderConfig()
        {
            CreateMap<Order, OrderGetDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedDate = src.CreatedDate.ToString("dd MMM yyyy hh.mmtt");
                dest.ClientData = $"{src.ClientName}\n{src.ClientPhone1}\n{src.ClientPhone2}";
                dest.Governrate = src.Government.Name;
                dest.City = src.City.Name;
            }).ReverseMap();
            
            
            CreateMap<OrderCreateDTO, Order>().AfterMap((src, dest) =>
            {
                dest.OrderType = (OrderType) Enum.Parse(typeof(OrderType), (src.OrderType));
                dest.PaymentType = (PaymentTypee) Enum.Parse(typeof(PaymentTypee), (src.PaymentType));
            }).ReverseMap();
        }
    }
}
