using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
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
            var deliveries = await deliveryService.GetAllAsync();
            if (deliveries == null || !deliveries.Any())
            {
                return NotFound("No deliveries found.");
            }
            List<DeliveryDTO> deliveryDTO = new List<DeliveryDTO>();
            DeliveryDTO dto;

            foreach (var delivery in deliveries) 
            {
                dto = new DeliveryDTO()
                {
                    Address = delivery.ApplicationUser?.Address,
                    Email = delivery.ApplicationUser?.Email,
                    Phone=delivery.ApplicationUser?.PhoneNumber,
                    Name=delivery.ApplicationUser?.UserName
                };
                deliveryDTO.Add(dto);
            }
            return Ok(deliveryDTO);
        }
      

      [HttpPost]
    public async Task<IActionResult> AddDelivery(DeliveryDTO deliveryDTO)
    {
        if (deliveryDTO == null)
        {
            return BadRequest("Delivery data is missing.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            //try
            //{
                // التحقق من المحافظات
                var allGovernmentsIdExist = await deliveryService.GetAllGovernmentExist(deliveryDTO.GovernmentsId);
                if (allGovernmentsIdExist == null || !allGovernmentsIdExist.Any())
                {
                    return NotFound("No governments found.");
                }

                var invalidIds = deliveryDTO.GovernmentsId.Except(allGovernmentsIdExist.Select(g => g.Id)).ToList();
                if (invalidIds.Any())
                {
                    return BadRequest($"Invalid government IDs: {string.Join(", ", invalidIds)}");
                }

                // التحقق من الفرع
                var branch = await branchService.GetByIdAsync(deliveryDTO.BranchId);
                if (branch == null)
                {
                    return NotFound("Branch not found.");
                }

                // التحقق من وجود المستخدم مسبقًا
                var existingUser = await userManager.FindByEmailAsync(deliveryDTO.Email);
                if (existingUser != null)
                {
                    return BadRequest("A user with this email already exists.");
                }

                // إنشاء مستخدم جديد
                var user = new ApplicationUser()
                {
                    UserName = deliveryDTO.Name,
                    Email = deliveryDTO.Email,
                    PhoneNumber = deliveryDTO.Phone,
                    Address = deliveryDTO.Address,
                };

                // حفظ المستخدم في قاعدة البيانات
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // إنشاء التوصيل وربطه بالمستخدم
                var delivery = new Delivery()
                {
                    AppUser_Id = user.Id,
                    Branch_Id = deliveryDTO.BranchId,
                    IsDeleted = deliveryDTO.IsDeleted
                };

                delivery.DeliveryGovernments.AddRange(deliveryDTO.GovernmentsId.Select(id => new DeliveryGovernment { Government_Id = id }));

               
                await deliveryService.AddAsync(delivery);
                await deliveryService.SaveChangesAsync();

                // تأكيد الحفظ وإنهاء الترانزاكشن
                transaction.Complete();

                return Ok(delivery);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"An error occurred: {ex.Message}");
            //}
        }
    }


}
}
