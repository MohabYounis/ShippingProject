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
            var query = unitOfWork.GetRepository<Merchant>().GetAllAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Orders)
                .Include(e => e.BranchMerchants).ThenInclude(n => n.Branch)
                .ToList();
        }

        public override async Task<IEnumerable<Merchant>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<Merchant>().GetAllExistAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Orders)
                .Include(e => e.BranchMerchants).ThenInclude(n => n.Branch)
                .ToList();
        }

        public async Task<Merchant> GetAllExistByPhoneNumberAsync(string phone)
        {
            var query = await unitOfWork.GetRepository<Merchant>().GetAllExistAsync();
            var employees = query.FirstOrDefault(m => m.ApplicationUser.PhoneNumber == phone);
            return employees;
        }
    }
}
