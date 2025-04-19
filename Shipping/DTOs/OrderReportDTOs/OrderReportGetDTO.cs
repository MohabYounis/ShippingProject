using Shipping.DTOs.OrderDTOs;

namespace Shipping.DTOs.OrderReportDTOs
{
    public class OrderReportGetDTO
    {
        public string SerialNumber { get; set; }
        public string OrderStatus { get; set; }
        public string MerchantName { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string Governrate { get; set; }
        public string City { get; set; }
        public decimal OrderCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal? DeliveryRight { get; set; }
        public decimal? CompanyRight { get; set; }
        public string CreatedDate { get; set; }
    }
}
