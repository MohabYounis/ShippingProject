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
                dest.ClientData = $"{src.ClientName}-{src.ClientPhone1}-{src.ClientPhone2}";
                dest.Governrate = src.Government.Name;
                dest.City = src.City.Name;
                dest.OrderStatus = src.OrderStatus.ToString();
            }).ReverseMap();

            CreateMap<Order, OrderGetDetailsDTO>()
            .ForMember(dest => dest.Merchant, opt => opt.MapFrom(src => src.Merchant != null ? src.Merchant.ApplicationUser.UserName : ""))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : ""))
            .ForMember(dest => dest.ShippingType, opt => opt.MapFrom(src => src.ShippingType != null ? src.ShippingType.Type : ""))
            .ForMember(dest => dest.Delivery, opt => opt.MapFrom(src => src.Delivery != null ? src.Delivery.ApplicationUser.UserName : "Pending delivery assignment"))
            .ForMember(dest => dest.Government, opt => opt.MapFrom(src => src.Government != null ? src.Government.Name : ""))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City != null ? src.City.Name : ""))
            .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType.ToString()))
            .ForMember(dest => dest.DeliverToVillage, opt => opt.MapFrom(src => src.DeliverToVillage ? "Yes" : "No"))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.ToString()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd HH:mm")));
        
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
