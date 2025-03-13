namespace Shipping.Models
{
    public class ShippingToVillage
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
