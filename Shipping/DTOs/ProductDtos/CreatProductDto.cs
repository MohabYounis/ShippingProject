using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.ProductDtos
{
    public class CreatProductDto
    {   
        [Required(ErrorMessage = "Product Id is required.")]
        [ForeignKey("product")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Item Weight is required.")]
        public float ItemWeight { get; set; }
    }
}
