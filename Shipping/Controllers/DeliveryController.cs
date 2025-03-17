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
            var deliveries = await deliveryService.GetAllDeliveryWithGovernmentsAsync();
            if (deliveries == null || !deliveries.Any())
            {
                return NotFound("No deliveries found.");
            }

            List<ShowDeliveryDto> deliveryDTO = deliveries.Select(delivery => new ShowDeliveryDto
            {
                Id = delivery.Id,
                Address = delivery.ApplicationUser?.Address,
                Email = delivery.ApplicationUser?.Email,
                Phone = delivery.ApplicationUser?.PhoneNumber,
                Name = delivery.ApplicationUser?.UserName,
                BranchName = delivery.Branch?.Name,
                GovernmentName = delivery.DeliveryGovernments.Select(dg => dg.Government?.Name).ToList(),
                IsDeleted = delivery.IsDeleted
            }).ToList();

            return Ok(deliveryDTO);

        }


        [HttpPost]
        public async Task<IActionResult> AddDelivery(DeliveryDTO deliveryDTO)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDelivery(int id, [FromBody] DeliveryDTO deliveryDTO)
        {
            if (deliveryDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await deliveryService.UpdateDeliveryAsync(id, deliveryDTO);
            if (!result)
            {
                return NotFound("Delivery not found or could not be updated.");
            }

            return Ok("Delivery updated successfully.");
        }

        [HttpGet("id")]
        public async Task<IActionResult>GetById(int id)
        {
            var delivery = await deliveryService.GetDeliveryByIdAsync(id);

            if (delivery == null)
            {
                return NotFound("Delivery not found.");
            }
            var Dto= new ShowDeliveryDto
            {
                Id = delivery.Id,
                Name = delivery.ApplicationUser?.UserName,
                Email = delivery.ApplicationUser?.Email,
                Phone = delivery.ApplicationUser?.PhoneNumber,
                Address = delivery.ApplicationUser?.Address,
                BranchName = delivery.Branch?.Name,
                GovernmentName = delivery.DeliveryGovernments?.Select(dg => dg.Government.Name).ToList(),
                IsDeleted = delivery.IsDeleted
            };

            return Ok(Dto);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var delivery = await deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null)
            {
                return NotFound("Delivery Not Found");
            }
            if (delivery.IsDeleted)
            {
                return BadRequest("Deliver is aready deleted");
            }
            delivery.IsDeleted = true;
            await deliveryService.UpdateAsync(id);
           //await deliveryService.DeleteAsync(id);
           await deliveryService.SaveChangesAsync();
            
            return Ok("Deleted Succes");
        }



    }
}
