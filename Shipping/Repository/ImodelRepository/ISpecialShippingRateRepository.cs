using Shipping.Models;
using Shipping.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Shipping.ImodelRepository
{
    public interface ISpecialShippingRateRepository : IRepositoryGeneric<SpecialShippingRate>
    {
        Task<SpecialShippingRate> GetSpecialRateByMerchantAsync(int merchantId, int cityId);
    }
}
