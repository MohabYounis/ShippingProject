using AutoMapper;
using Shipping.DTOs.OrderDTOs;
using Shipping.Models;

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
        }
    }
}
