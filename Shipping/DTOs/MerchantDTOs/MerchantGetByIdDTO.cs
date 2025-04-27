using Shipping.DTOs.SpecialShippingRatesDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.MerchantDTOs
{
    public class MerchantGetByIdDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CreatedDate { get; set; }
        [MaxLength(100)]
        [Display(Name = "Store Name")]
        public string StoreName { get; set; }
        [Display(Name = "State")]
        public bool IsDeleted { get; set; }
        [Display(Name = "Governorate")]
        public string Government { get; set; }
        public string City { get; set; }
        [Display(Name = "Special Pickup Cost")]
        public decimal PickupCost { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100%")]
        [Display(Name = "Rejected Order Percentage")]
        public decimal RejectedOrderPercentage { get; set; }
        [Display(Name = "Branches")]
        public List<SpecialCreateDTO>? SpecialShippingRates { get; set; }
        public List<int>? Branches_Id { get; set; }
    }
}
