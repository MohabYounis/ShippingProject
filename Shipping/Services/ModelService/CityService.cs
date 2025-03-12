using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class CityService : ServiceGeneric<City>, ICityService
    {
        public CityService(IRepositoryGeneric<City> repository) : base(repository)
        {
        }
    }
}
