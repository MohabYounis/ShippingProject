using SHIPPING.Repository;

namespace SHIPPING.ModelRepository
{
    public class  : GenericRepository<WeightPricing>
    {
        public (ShippingContext context) : base(context)
        {
        }
    }
}
