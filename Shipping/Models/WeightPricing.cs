namespace Shipping.Models
{
    public class WeightPricing // add one time then update
    {
        public int Id { get; set; }
        public float DefaultWeight { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal AdditionalKgPrice { get; set; }
        public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
