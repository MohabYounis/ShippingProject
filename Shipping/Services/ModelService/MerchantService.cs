using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipping.Services.ModelService
{
    public class MerchantService : ServiceGeneric<Merchant>, IMerchantService
    {
        public MerchantService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<IEnumerable<Merchant>> GetAllAsync()
        {
            var query = unitOfWork.GetRepository<Merchant>().GetAllExistAsync();
            var employees = await query;
            return employees
                .Include(e => e.Government)
                .Include(e => e.Orders)
                .Include(e => e.SpecialShippingRates)
                .ToList();
        }

        public override async Task<IEnumerable<Merchant>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<Merchant>().GetAllExistAsync();
            var employees = await query;
            return employees
                .Include(e => e.Government)
                .Include(e => e.Orders)
                .Include(e => e.SpecialShippingRates)
                .ToList();
        }
    }
}
