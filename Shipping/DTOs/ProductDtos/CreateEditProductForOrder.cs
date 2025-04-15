using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.ProductDtos
{
    public class CreateEditProductForOrder
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Item Weight is required.")]
        public float ItemWeight { get; set; }
    }
}
