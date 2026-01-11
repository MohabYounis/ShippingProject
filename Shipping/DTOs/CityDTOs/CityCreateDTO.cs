using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.CityDTOs
{
    public class CityCreateDTO
    {
        public int Government_Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters.")]
        [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "PickupShipping must be a positive number.")]
        public decimal? PickupShipping { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "StandardShipping must be a positive number.")]
        public decimal? StandardShipping { get; set; }
    }
}
