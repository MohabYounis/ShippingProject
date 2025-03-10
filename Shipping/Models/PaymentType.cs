namespace Shipping.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
