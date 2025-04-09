using Shipping.DTOs.NewFolder1;
using Shipping.DTOs.SpecialShippingRatesDTOs;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.MerchantDTOs
{
    public class MerchantEditDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",ErrorMessage = "New password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 100 characters.")]
        public string Address { get; set; }

        [MaxLength(100)]
        //[Required(ErrorMessage = "Store name is required.")]
        public string StoreName { get; set; }

        [MaxLength(100)]
        [MinLength(6, ErrorMessage = "Government name must be at least 3 characters long.")]
        [Required(ErrorMessage = "Government is required.")]
        public string Government { get; set; }

        [MaxLength(100)]
        [MinLength(6, ErrorMessage = "City name must be at least 3 characters long.")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "PickupCost is required.")]
        public decimal PickupCost { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100%")]
        public decimal RejectedOrderPercentage { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<SpecialCreateDTO>? SpecialShippingRates { get; set; }
        public List<int>? Branches_Id { get; set; }
    }
}
