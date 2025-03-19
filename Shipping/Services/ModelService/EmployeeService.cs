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
            var query = unitOfWork.GetRepository<Employee>().GetAllAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToList();
        }

        public override async Task<IEnumerable<Employee>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToList();
        }
    }
}
