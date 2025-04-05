using Shipping.DTOs.SpecialShippingRatesDTOs;
using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.NewFolder1
{
    public class MerchantCreateDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
        [MaxLength(100)]    
        public string StoreName { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public decimal PickupCost { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100%")]
        public decimal RejectedOrderPercentage { get; set; }
        public List<SpecialCreateDTO>? SpecialShippingRates { get; set; }
        public List<int>? Branches_Id { get; set; }
    }
}
