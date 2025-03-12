using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class PaymentTypeService : ServiceGeneric<PaymentType>, IPaymentTypeService
    {
        public PaymentTypeService(IRepositoryGeneric<PaymentType> repository) : base(repository)
        {
        }
    }
}
