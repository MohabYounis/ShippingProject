using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class City
    {
        public int Id { get; set; }
        [ForeignKey("Government")]
        public int Government_Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public decimal? PickupShipping {  get; set; }
        public decimal? StandardShipping {  get; set; }
        public virtual Government? Government { get; set; }
        public virtual List<Order>? Orders { get; } = new List<Order>();
        public virtual List<SpecialShippingRate>? SpecialShippingRates { get; } = new List<SpecialShippingRate>();
    }
}
