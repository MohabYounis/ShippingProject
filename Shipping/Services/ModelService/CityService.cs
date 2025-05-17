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
            var employees = await unitOfWork.GetRepository<City>().GetAllAsync();
            return employees
                .Include(e => e.SpecialShippingRates)
                .Include(e => e.Orders)
                .Include(e => e.Government)
                .ToList();
        }

        public override async Task<IEnumerable<City>> GetAllExistAsync()
        {
            var employees = await unitOfWork.GetRepository<City>().GetAllExistAsync();
            return employees
                .Include(e => e.SpecialShippingRates)
                .Include(e => e.Orders)
                .Include(e => e.Government)
                .ToList();
        }

        public override async Task<City> GetByIdAsync(int id)
        {
            var query = await unitOfWork.GetRepository<City>().GetAllAsync();
            return await query.Include(e => e.Government)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
