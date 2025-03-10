namespace Shipping.Models
{
    public enum CostLevel
    {
        Low,
        Medium,
        High,
    } 
    public class ShippingType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public CostLevel CostLevel  { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
