using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace Shipping.Services.ModelService
{
    public class EmployeeService : ServiceGeneric<Employee>, IEmployeeService
    {

        public EmployeeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var query = await unitOfWork.GetRepository<Employee>().GetAllAsync();
            return await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Employee>> GetAllExistAsync()
        {
            var query = await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            return await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToListAsync();
        }



    }
}
