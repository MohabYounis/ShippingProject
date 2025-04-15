using Shipping.DTOs.ProductDtos;
using Shipping.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.OrderDTOs
{
    public class OrderCreateEditDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Merchant ID must be a positive number.")]
        public int Merchant_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Branch ID must be a positive number.")]
        public int Branch_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Government ID must be a positive number.")]
        public int Government_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Shipping type ID must be a positive number.")]
        public int ShippingType_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "City ID must be a positive number.")]
        public int City_Id { get; set; }

        [StringLength(50, ErrorMessage = "Order type must not exceed 50 characters.")]
        public string? OrderType { get; set; }

        [Required(ErrorMessage = "Client name is required.")]
        [StringLength(100, ErrorMessage = "Client name must not exceed 100 characters.")]
        public string? ClientName { get; set; }

        [Required(ErrorMessage = "Client phone is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string? ClientPhone1 { get; set; }

        [RegularExpression(@"^(?:\+20|0)?1[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string? ClientPhone2 { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? ClientEmail { get; set; }

        [Required(ErrorMessage = "Client address is required.")]
        [StringLength(200, ErrorMessage = "Client address must not exceed 200 characters.")]
        public string? ClientAddress { get; set; }
        public bool DeliverToVillage { get; set; }

        [Required(ErrorMessage = "Payment type is required.")]
        [StringLength(50, ErrorMessage = "Payment type must not exceed 50 characters.")]
        public string? PaymentType { get; set; }

        [Required(ErrorMessage = "Order cost is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Order cost must be a positive value.")]
        public decimal OrderCost { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Order weight must be a positive value.")]
        public float OrderTotalWeight { get; set; }

        [StringLength(250)]
        public string? MerchantNotes { get; set; }

        [StringLength(250)]
        public string? EmployeeNotes { get; set; }

        [StringLength(250)]
        public string? DeliveryNotes { get; set; }
        public List<CreatProductDto>? Products { get; set; }
    }
}
