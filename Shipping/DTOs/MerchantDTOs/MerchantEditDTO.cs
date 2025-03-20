using Shipping.DTOs.NewFolder1;
using Shipping.DTOs.SpecialShippingRatesDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.MerchantDTOs
{
    public class MerchantEditDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [MaxLength(100)]
        public string StoreName { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public decimal PickupCost { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100%")]
        public decimal RejectedOrderPercentage { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<SpecialCreateDTO>? SpecialShippingRates { get; set; }
        public List<int>? Branches_Id { get; set; }
    }
}
