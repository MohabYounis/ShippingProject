using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    [PrimaryKey(nameof(Product_Id), nameof(Order_Id))]
    public class OrderProduct
    {
        public int SubTotalWeight { get; set; } // product weight * product quantity
        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        [ForeignKey("Order")]
        public int Order_Id { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Order? Order { get; set; }
    }
}
