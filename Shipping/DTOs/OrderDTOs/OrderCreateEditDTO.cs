using Shipping.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.OrderDTOs
{
    public class OrderCreateEditDTO
    {
        public int Merchant_Id { get; set; }
        public int Branch_Id { get; set; }
        public int Government_Id { get; set; }
        public int ShippingType_Id { get; set; }
        public int City_Id { get; set; }
        public string? OrderType { get; set; }
        public string? ClientName { get; set; }
        public string? ClientPhone1 { get; set; }
        public string? ClientPhone2 { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientAddress { get; set; }
        public bool DeliverToVillage { get; set; }
        public string? PaymentType { get; set; }
        public decimal OrderCost { get; set; }
        public float OrderTotalWeight { get; set; }
        public string? MerchantNotes { get; set; }
        public string? EmployeeNotes { get; set; }
        public string? DeliveryNotes { get; set; }
        public List<Product>? Products { get; set; }
    }
}
