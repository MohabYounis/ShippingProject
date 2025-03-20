using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
namespace Shipping.Services.ModelService
{
    public class CityService : ServiceGeneric<City>, ICityService
    {
        public CityService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public override async Task<IEnumerable<City>> GetAllAsync()
        {
            var query = unitOfWork.GetRepository<City>().GetAllAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.SpecialShippingRates)
                .Include(e => e.Orders)
                .Include(e => e.BranchMerchants).ThenInclude(e => e.Branch)
                .ToList();
        }

        public override async Task<IEnumerable<City>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<City>().GetAllExistAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.SpecialShippingRates)
                .Include(e => e.Orders)
                .Include(e => e.BranchMerchants).ThenInclude(e => e.Branch)
                .ToList();
        }
    }
}
