using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.GovernmentDTOs
{
    public class GovernmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        //[Required(ErrorMessage = "Branch ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Branch ID must be greater than 0.")]
        public int Branch_Id { get; set; }
    }
}

