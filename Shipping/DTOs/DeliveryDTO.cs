using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs
{
    public class DeliveryDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 10 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Branch ID is required.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "At least one government ID is required.")]
        public List<int> GovernmentsId { get; set; }

        public bool IsDeleted { get; set; } = false; 
    }

}

