using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.ProductDtos
{
    public class ProductDTO
    {
        public int Id { get; set; }

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

        public bool IsDeleted { get; set; }

    }
}
