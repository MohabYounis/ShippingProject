using Shipping.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.OrderDTOs
{
    public class OrderGetDetailsDTO
    {
        public string SerialNumber { get; set; }
        public string Merchant { get; set; }
        public string Branch { get; set; }
        public string ShippingType { get; set; }
        public string Delivery { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public string OrderType { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone1 { get; set; }
        public string ClientPhone2 { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public string DeliverToVillage { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentType { get; set; }
        public string CreatedDate { get; set; }
        public decimal OrderCost { get; set; }
        public decimal ShippingCost { get; set; }
        public float OrderTotalWeight { get; set; }
        public string? MerchantNotes { get; set; }
        public string? EmployeeNotes { get; set; }
        public string? DeliveryNotes { get; set; }
    }
}
