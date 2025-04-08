using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.DeliveryDTOs
{
    public class DeliveryCreateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 10 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5,1]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string Phone { get; set; }
        //password 
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Branch ID is required.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "At least one government ID is required.")]
        public List<int> GovernmentsId { get; set; }

        [Required(ErrorMessage = "Choose a discount type.")]
        public string DiscountType { get; set; }                                        //added by Mohab

        [Required(ErrorMessage = "Select the company percentage from order.")]
        public decimal CompanyPercentage { get; set; }                                  //added by Mohab
    }

}

