using Microsoft.EntityFrameworkCore.Storage;
using Shipping.DTOs.Employee;
using Shipping.DTOs.pagination;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IEmployeeService : IServiceGeneric<Employee>
    {

        Task<GenericPagination<EmployeeDTO>> GetAllAsync(int pageIndex = 1, int pageSize = 10);       
        Task<GenericPagination<EmployeeDTO>> GetAllExistAsync(int pageIndex = 1, int pageSize = 10);       
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<IEnumerable<Employee>> GetEmployeesByRole(string roleName);


        Task<IEnumerable<Employee>> GetEmployeesBySearch(string term, bool includeDelted = true);


    }
}
