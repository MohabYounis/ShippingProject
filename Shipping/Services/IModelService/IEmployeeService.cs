using Microsoft.EntityFrameworkCore.Storage;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IEmployeeService :IServiceGeneric<Employee>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
