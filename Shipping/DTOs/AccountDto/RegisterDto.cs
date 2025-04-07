using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.AccountDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "Username must not exceed 20 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5,1]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [MaxLength(20, ErrorMessage = "Password must not exceed 20 characters.")]
        public string Password { get; set; }

        [MaxLength(100, ErrorMessage = "Address must not exceed 100 characters.")]
        public string Address { get; set; }

        public bool? Gender { get; set; } 

        public bool? IsMerchant { get; set; } = false;
        public bool? IsDelivery { get; set; } = false;
        public bool? IsEmployee { get; set; } = false;
    }
}
