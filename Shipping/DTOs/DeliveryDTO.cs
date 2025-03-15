using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs
{
    public class DeliveryDTO
    {
        [Required]
        [MaxLength(10)]
        [MinLength(3)]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public string Phone { get; set; }
        public string Address { get; set; }


        public List<Branch> BranchName { get; set; }
        public List<Government> Governments { get; set; }

    }
}
