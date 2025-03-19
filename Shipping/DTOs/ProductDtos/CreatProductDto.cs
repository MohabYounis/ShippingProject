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
        [StringLength(100, ErrorMessage = "Name length can't be more than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Item Weight is required.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Item Weight must be greater than 0.")]
        public float ItemWeight { get; set; }
    }
}
