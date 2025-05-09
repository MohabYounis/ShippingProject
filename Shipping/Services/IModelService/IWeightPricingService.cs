﻿using Shipping.DTOs;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IWeightPricingService : IServiceGeneric<WeightPricing>
    {
         Task<WeightPricingDTO> AddWeightAsync(WeightPricingDTO weightPricing);
         Task<WeightPricing> UpdateWeightAsync(WeightPricing weightPricing);
    }
}
