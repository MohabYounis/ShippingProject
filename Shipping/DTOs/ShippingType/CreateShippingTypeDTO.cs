using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.ShippingType
{
    public class CreateShippingTypeDTO
    {
        [Required(ErrorMessage = "Shipping type is required.")]
      
        public string Type { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Shipping type cost is required.")]
        public decimal Cost { get; set; }

    }
}
