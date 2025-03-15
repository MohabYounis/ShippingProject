using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Product
    {
        public int Id { get; set; }
        [ForeignKey("product")]
        public int Product_Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float ItemWeight { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Product? product { get; set; }
    }
}
