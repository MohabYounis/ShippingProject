using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Shipping.ImodelRepository;

namespace Shipping.Services.ModelService
{
    public class SpecialShippingRateService : ServiceGeneric<SpecialShippingRate>, ISpecialShippingRateService
    {
        ISpecialShippingRateRepository specialShippingRateRepository;
        public SpecialShippingRateService(IUnitOfWork unitOfWork, ISpecialShippingRateRepository specialShippingRateRepository) : base(unitOfWork)
        {
            this.specialShippingRateRepository = specialShippingRateRepository;
        }

        public async Task<SpecialShippingRate> GetSpecialRateByMerchant(int merchantId, int cityId)
        {
            return await unitOfWork.SpecialShippingRateRepository.GetSpecialRateByMerchantAsync(merchantId, cityId);
        }
    }
}
