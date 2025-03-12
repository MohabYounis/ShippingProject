using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class EmployeeService : ServiceGeneric<Employee>, IEmployeeService
    {
        public EmployeeService(IRepositoryGeneric<Employee> repository) : base(repository)
        {
        }
    }
}
