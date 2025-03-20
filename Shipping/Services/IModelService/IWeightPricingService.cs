using Shipping.DTOs;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IWeightPricingService
    {
         Task<WeightPricing> UpdateWeightAsync(WeightPricing weightPricing);
    }
}
