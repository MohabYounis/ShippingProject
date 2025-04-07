using Shipping.Models;
using Shipping.Repository;
using Shipping.Services.IModelService;
using SHIPPING.Services;
using Shipping.UnitOfWorks;
using Shipping.DTOs;
using AutoMapper;

public class WeightPricingService : ServiceGeneric<WeightPricing>, IWeightPricingService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IRepositoryGeneric<WeightPricing> repositoryGeneric;
    private readonly IMapper mapper;

    public WeightPricingService(IUnitOfWork unitOfWork, IRepositoryGeneric<WeightPricing> repositoryGeneric , IMapper mapper)
        : base(unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        this.repositoryGeneric = repositoryGeneric;
        this.mapper = mapper;
    }

    public async Task<WeightPricingDTO> AddWeightAsync(WeightPricingDTO weightPricing)
    {
        if (weightPricing == null) throw new ArgumentNullException(nameof(weightPricing));

        // Check if there's already an existing weight pricing record
        var existingWeightPricing = await unitOfWork.GetRepository<WeightPricing>().GetAllAsync();
        var firstWeightPricing = existingWeightPricing.FirstOrDefault();

        if (firstWeightPricing != null)
        {
            // If a record exists, return a message or trigger an update
            throw new InvalidOperationException("Weight pricing already exists. Please update the existing record.");
        }

        // If no existing record is found, add the new weight pricing
        var result = mapper.Map<WeightPricing>(weightPricing);
        await repositoryGeneric.AddAsync(result);
        await unitOfWork.SaveChangesAsync();

        return weightPricing;  // Return the newly added weightPricing
    }

   public async Task<WeightPricing> UpdateWeightAsync(WeightPricing weightPricing)
{
    if (weightPricing == null) throw new ArgumentNullException(nameof(weightPricing));

    // Retrieve the existing record to update
    var allweight = await unitOfWork.GetRepository<WeightPricing>().GetAllAsync();
    var firstWeightPricing = allweight.FirstOrDefault();

    if (firstWeightPricing == null)
    {
        // If no record exists, throw an error indicating no data to update
        throw new InvalidOperationException("No existing weight pricing to update.");
    }

    // Update the existing record with the new values
    firstWeightPricing.DefaultWeight = weightPricing.DefaultWeight;
    firstWeightPricing.AdditionalKgPrice = weightPricing.AdditionalKgPrice;

    repositoryGeneric.Update(firstWeightPricing);
    await unitOfWork.SaveChangesAsync();

    return firstWeightPricing;
}

}
