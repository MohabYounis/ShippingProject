using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.DTOs.DeliveryDTOs;
using Shipping.DTOs.GovernmentDTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using System.Transactions;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService deliveryService;
        private readonly IServiceGeneric<Branch> branchService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IServiceGeneric<Government> governmentService;

        public DeliveryController(IDeliveryService DeliveryService,IServiceGeneric<Branch>branchService, UserManager<ApplicationUser> userManager, IServiceGeneric<Government> governmentService)
        {
            deliveryService = DeliveryService;
            this.branchService = branchService;
            this.userManager = userManager;
            this.governmentService = governmentService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var delivery = await deliveryService.GetDeliveryByIdAsync(id);

            if (delivery == null) return NotFound("Not Found.");

            var Dto = new ShowDeliveryDto
            {
                Id = delivery.Id,
                Name = delivery.ApplicationUser?.UserName,
                Email = delivery.ApplicationUser?.Email,
                Phone = delivery.ApplicationUser?.PhoneNumber,
                Address = delivery.ApplicationUser?.Address,
                BranchName = delivery.Branch?.Name,
                GovernmentName = delivery.DeliveryGovernments?.Select(dg => dg.Government.Name).ToList(),
                DiscountType = delivery.DiscountType.ToString(),
                CompanyPercentage = delivery.CompanyPercentage,
                IsDeleted = delivery.IsDeleted
            };

            return Ok(Dto);
        }

        [HttpGet("{all:alpha}")]
        public async Task<ActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Delivery> delivery;
                if (all == "all") delivery = await deliveryService.GetAllDeliveryWithGovernmentsAsync();
                else if (all == "exist") delivery = await deliveryService.GetAllExistAsync();
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (delivery == null || !delivery.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching by name or phone
                        delivery = delivery
                            .Where(item =>
                                (item.ApplicationUser.UserName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ApplicationUser.PhoneNumber?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!delivery.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    var totalMerchnts = delivery.Count();

                    // Pagination
                    var paginatedMerchants = delivery
                        .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                    var deliveryExist = delivery.Where(i => !i.IsDeleted).ToList();

                    List<ShowDeliveryDto> deliveryDTO = deliveryExist.Select(delivery => new ShowDeliveryDto
                    {
                        Id = delivery.Id,
                        Address = delivery.ApplicationUser?.Address,
                        Email = delivery.ApplicationUser?.Email,
                        Phone = delivery.ApplicationUser?.PhoneNumber,
                        Name = delivery.ApplicationUser?.UserName,
                        BranchName = delivery.Branch?.Name,
                        GovernmentName = delivery.DeliveryGovernments.Select(dg => dg.Government?.Name).ToList(),
                        DiscountType = delivery.DiscountType.ToString(),
                        CompanyPercentage = delivery.CompanyPercentage,
                        IsDeleted = delivery.IsDeleted
                    }).ToList();

                    return Ok(deliveryDTO);

                    return Ok(GeneralResponse.Success(deliveryDTO));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }

        [HttpGet("Branch/{branchId:int}")]
        public async Task<IActionResult> GetByBranchId(int branchId)
        {
            var branch = await branchService.GetByIdAsync(branchId);
            if (branch == null) return NotFound("Branch not found.");

            var deliveries = await deliveryService.GetDeliveryByBranchIdAsync(branchId);
            if (deliveries == null || !deliveries.Any()) return NotFound("No deliveries found.");

            var deliveryDtos = deliveries.Select(delivery => new ShowDeliveryDto
            {
                Id = delivery.Id,
                Name = delivery.ApplicationUser?.UserName,
                Email = delivery.ApplicationUser?.Email,
                Phone = delivery.ApplicationUser?.PhoneNumber,
                Address = delivery.ApplicationUser?.Address,
                BranchName = delivery.Branch?.Name,
                GovernmentName = delivery.DeliveryGovernments?
                                    .Select(dg => dg.Government.Name)
                                    .ToList(),
                DiscountType = delivery.DiscountType.ToString(),
                CompanyPercentage = delivery.CompanyPercentage,
                IsDeleted = delivery.IsDeleted
            }).ToList();

            return Ok(deliveryDtos);
        }
        [HttpGet("GovernmentByBranch/{branchId:int}")]
        public async Task<IActionResult> GetGovernmentByBranch(int branchId)
        {
            var branch = await branchService.GetByIdAsync(branchId);
            if (branch == null) return NotFound("Branch not found.");

            var government = await deliveryService.GetGovernmentByBranchId(branchId);
            if (government == null || !government.Any()) return NotFound("No deliveries found.");

            var deliveryDtos = government.Select(government => new GovernmentDTO
            {
                Id = government.Id,
                Name = government.Name,
                Branch_Id=government.Branch_Id,
                IsDeleted=government.IsDeleted
            }).ToList();

            return Ok(deliveryDtos);
        }




        [HttpPost]
        public async Task<IActionResult> AddDelivery(DeliveryCreateDTO deliveryDTO)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest("null parameter");
            }
            try
            {
                var result = await deliveryService.AddDeliveryAsync(deliveryDTO);
                if (result)
                {
                    return Ok("Delivery added successfully.");
                }
                return BadRequest("Failed to add delivery.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDelivery(int id, DeliveryEditDTO deliveryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await deliveryService.UpdateDeliveryAsync(id, deliveryDTO);
            if (!result)
            {
                return NotFound("Delivery not found or could not be updated.");
            }

            return Ok("Delivery updated successfully.");
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var delivery = await deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null) return NotFound("Not Found.");
            if (delivery.IsDeleted) return BadRequest(new { success = false, message = "Delivery is already deleted." });


            await deliveryService.DeleteAsync(id);
            await deliveryService.SaveChangesAsync();

            return Ok(new { success = true, message = "Delivery deleted successfully." });
        }
    }
}
