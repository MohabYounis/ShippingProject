using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Product
    {
        public int Id { get; set; }
        [ForeignKey("Merchant")]
        public int Merchant_Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Merchant? Merchant { get; set; }
        public virtual List<OrderProduct>? OrderProducts { get; } = new List<OrderProduct>();
    }
}
