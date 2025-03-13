using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class ProductService : ServiceGeneric<Product>, IProductService
    {
        public ProductService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
