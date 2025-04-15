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
        Task<IEnumerable<EmployeeDTO>> GetEmployeesByRole(string roleName);

        Task<EmployeeDTO> UpdateAsync(int id, UpdateEmployeeDTO employeeDto);

        Task<EmployeeDTO> AddAsync(CreateEmployeeDTO employeeDto);

        Task<string> DeleteAsync(int id);
        
        Task<EmployeeDTO> GetByIdAsync(int id);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesBySearch(string term, bool includeDelted = true);


    }
}
