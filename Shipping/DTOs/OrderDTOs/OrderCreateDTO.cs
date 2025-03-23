using Shipping.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        public int Merchant_Id { get; set; }
        public int Branch_Id { get; set; }
        public int Government_Id { get; set; }
        public int ShippingType_Id { get; set; }
        public int City_Id { get; set; }
        public string OrderType { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone1 { get; set; }
        public string ClientPhone2 { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public bool DeliverToVillage { get; set; }
        public string PaymentType { get; set; }
        public decimal OrderCost { get; set; }
        public float OrderTotalWeight { get; set; }
        public string Notes { get; set; }
        public string MerchantPhone {  get; set; }
        public string MerchantAddress { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
