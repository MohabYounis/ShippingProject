using AutoMapper;
using Shipping.DTOs.OrderDTOs;
using Shipping.DTOs.ProductDtos;
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


            //CreateMap<OrderCreateEditDTO, Order>().AfterMap((src, dest) =>
            //{
            //    dest.OrderType = (OrderType) Enum.Parse(typeof(OrderType), (src.OrderType));
            //    dest.PaymentType = (PaymentTypee) Enum.Parse(typeof(PaymentTypee), (src.PaymentType));
            //}).ReverseMap();

            CreateMap<CreateEditProductForOrder, Product>().ReverseMap();

            CreateMap<OrderCreateEditDTO, Order>()
                .AfterMap((src, dest) =>
                {
                    // Parse enums safely (with fallback if needed)
                    if (!string.IsNullOrEmpty(src.OrderType))
                        dest.OrderType = (OrderType)Enum.Parse(typeof(OrderType), src.OrderType);

                    if (!string.IsNullOrEmpty(src.PaymentType))
                        dest.PaymentType = (PaymentTypee)Enum.Parse(typeof(PaymentTypee), src.PaymentType);

                    // Map products
                    if (src.Products != null)
                    {
                        dest.Products = src.Products.Select(p => new Product
                        {
                            Name = p.Name,
                            Quantity = p.Quantity,
                            ItemWeight = p.ItemWeight
                            // OrderId بيتحدد بعد الحفظ
                        }).ToList();
                    }
                })
                .ReverseMap();

        }
    }
}
