using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Branch
{
    public class CreateAndEditBranchDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Location { get; set; }

    }
}
