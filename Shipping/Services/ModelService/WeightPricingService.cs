﻿using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;

namespace Shipping.Services.ModelService
{
    public class WeightPricingService : ServiceGeneric<WeightPricing>, IWeightPricingService
    {
        public WeightPricingService(IRepositoryGeneric<WeightPricing> repository) : base(repository)
        {
        }
    }
}
