using Shipping.Models;
using Shipping.Repository;
using Shipping.Services.IModelService;
using SHIPPING.Services;
using Shipping.UnitOfWorks;

public class WeightPricingService : ServiceGeneric<WeightPricing>, IWeightPricingService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IRepositoryGeneric<WeightPricing> repositoryGeneric;

    public WeightPricingService(IUnitOfWork unitOfWork, IRepositoryGeneric<WeightPricing> repositoryGeneric)
        : base(unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        this.repositoryGeneric = repositoryGeneric;
    }

    public async Task<WeightPricing> UpdateWeightAsync(WeightPricing weightPricing)
    {
        if (weightPricing == null) throw new ArgumentNullException(nameof(weightPricing));

        var allweight = await unitOfWork.GetRepository<WeightPricing>().GetAllAsync();
        var firstWeightPricing = allweight.FirstOrDefault();

        if (firstWeightPricing != null)
        {
            firstWeightPricing.DefaultWeight = weightPricing.DefaultWeight;
            firstWeightPricing.AdditionalKgPrice = weightPricing.AdditionalKgPrice;

            repositoryGeneric.Update(firstWeightPricing);
            await unitOfWork.SaveChangesAsync();
        }

        return firstWeightPricing;
    }
}
