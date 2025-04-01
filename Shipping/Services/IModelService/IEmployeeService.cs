using Microsoft.EntityFrameworkCore.Storage;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IEmployeeService : IServiceGeneric<Employee>
    {

        Task<IEnumerable<Employee>> GetAllAsync(int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<Employee>> GetAllExistAsync(int pageIndex = 1, int pageSize = 10);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<IEnumerable<Employee>> GetEmployeesByRole(string roleName);


        Task<IEnumerable<Employee>> GetEmployeesBySearch(string term, bool includeDelted = true);


    }
}
