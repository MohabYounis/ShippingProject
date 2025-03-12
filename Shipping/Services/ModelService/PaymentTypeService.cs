using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class PaymentTypeService : ServiceGeneric<PaymentType>, IPaymentTypeService
    {
        public PaymentTypeService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
