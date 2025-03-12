using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class CityService : ServiceGeneric<City>, ICityService
    {
        public CityService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
