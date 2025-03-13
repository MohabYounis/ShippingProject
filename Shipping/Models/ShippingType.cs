namespace Shipping.Models
{
    public class ShippingType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Cost  { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
