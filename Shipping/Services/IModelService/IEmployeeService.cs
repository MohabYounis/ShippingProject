using Microsoft.EntityFrameworkCore.Storage;
using Shipping.DTOs.Employee;
using Shipping.DTOs.pagination;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IEmployeeService : IServiceGeneric<Employee>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task UpdateAsync(int id, UpdateEmployeeDTO employeeDto);
        Task AddAsync(CreateEmployeeDTO employeeDto);
        Task DeleteAsync(int id);
        Task<EmployeeGetDTO> GetByIdAsync(int id);

        Task<IEnumerable<EmployeeGetDTO>> GetAllAsync();
        Task<IEnumerable<EmployeeGetDTO>> GetAllExistAsync();
    }
}
