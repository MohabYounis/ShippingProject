using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs.DeliveryDTOs;
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

        public DeliveryController(IDeliveryService DeliveryService,IServiceGeneric<Branch>branchService, UserManager<ApplicationUser> userManager)
        {
            deliveryService = DeliveryService;
            this.branchService = branchService;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDelivery()
        {
            try
            {
                var deliveries = await deliveryService.GetAllDeliveryWithGovernmentsAsync();
                if (deliveries == null || !deliveries.Any()) return NotFound("Not Found.");

                List<ShowDeliveryDto> deliveryDTO = deliveries.Select(delivery => new ShowDeliveryDto
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
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("AllExist")]
        public async Task<IActionResult> GetAllExist()
        {
            try
            {
                var deliveries = await deliveryService.GetAllDeliveryWithGovernmentsAsync();
                var deliveryExist = deliveries.Where(i => !i.IsDeleted).ToList();

                if (deliveries == null || !deliveries.Any()) return NotFound("Not Found.");

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
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            if (delivery.IsDeleted) return BadRequest("Deliver is already deleted.");

            await deliveryService.DeleteAsync(id);
            await deliveryService.SaveChangesAsync();
            
            return Ok("Delivery deleted Successfully.");
        }
   
    
    
    }
}
