using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.NewFolder1;
using Shipping.Models;
using Shipping.Services;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        public IServiceGeneric<Merchant> service;
        IMapper mapper;
        GeneralResponse response;
        UserManager<ApplicationUser> userManager;
        public MerchantController(IServiceGeneric<Merchant> service, IMapper mapper, GeneralResponse response, UserManager<ApplicationUser> userManager)
        {
            this.service = service;
            this.mapper = mapper;
            this.response = response;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResponse>> GetAllMerchants()
        {
            var merchants = await service.GetAllAsync();
            List<MerchantGetDTO> merchantDTO = mapper.Map<List<MerchantGetDTO>>(merchants);

            if (merchants == null)
            {
                response.IsSuccess = false;
                response.Data = "No Merchants Exist";
            }
            else
            {
                response.IsSuccess = true;
                response.Data = merchantDTO;
            }
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> CreateMerchant(MerchantCreateDTO merchantFromReq)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
            }
            else
            {
                var newMerchant = new Merchant()
                {
                    StoreName = merchantFromReq.StoreName,
                    Government = merchantFromReq.Government,
                    City = merchantFromReq.City,
                    PickupCost = merchantFromReq.PickupCost,
                    RejectedOrderPercentage = merchantFromReq.RejectedOrderPercentage,
                };

                var newUser = new ApplicationUser()
                {
                    UserName = merchantFromReq.Name,
                    Email = merchantFromReq.Email,
                    Address = merchantFromReq.Address,
                    Merchant = newMerchant,
                };

                var result = await userManager.CreateAsync(newUser, merchantFromReq.Password);
                

                if (result.Succeeded)
                {
                    response.IsSuccess = true;
                    response.Data = "Merchant Created Successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Data = result.Errors.Select(e => e.Description).ToList();
                }
            }
            return response;
        }
    }
}
