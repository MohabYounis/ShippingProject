using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.ImodelRepository;
using Shipping.Models;
using Microsoft.EntityFrameworkCore;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.modelRepository
{
    public class SpecialShippingRateRepository : RepositoryGeneric<SpecialShippingRate>, ISpecialShippingRateRepository
    {
        public SpecialShippingRateRepository (IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<SpecialShippingRate> GetSpecialRateByMerchantAsync(int merchantId, int cityId)
        {
            return await Context.SpecialShippingRates
                .Include(rate => rate.Merchant)
                .Include(rate => rate.City)
                .FirstOrDefaultAsync(rate => rate.Merchant_Id == merchantId && rate.City_Id == cityId);
        }
    }
}

