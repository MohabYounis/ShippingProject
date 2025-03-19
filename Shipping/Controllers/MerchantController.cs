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


        [HttpGet("All")]
        public async Task<ActionResult<GeneralResponse>> GetAll()
        {
            try
            {
                var merchants = await service.GetAllAsync();
                if (merchants == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                }
                else
                {
                    List<MerchantGetDTO> merchantDTO = mapper.Map<List<MerchantGetDTO>>(merchants);
                    response.IsSuccess = true;
                    response.Data = merchantDTO;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }
        
        
        [HttpGet("AllExist")]
        public async Task<ActionResult<GeneralResponse>> GetAllExist()
        {
            try
            {
                var merchants = await service.GetAllExistAsync();

                if (merchants == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                }
                else
                {
                    List<MerchantGetDTO> merchantDTO = mapper.Map<List<MerchantGetDTO>>(merchants);
                    response.IsSuccess = true;
                    response.Data = merchantDTO;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> GetById(int id)
        {
            try
            {
                var merchant = await service.GetByIdAsync(id);
                MerchantGetDTO merchantDTO = mapper.Map<MerchantGetDTO>(merchant);

                response.IsSuccess = true;
                response.Data = merchantDTO;
            }
            catch (Exception ex)
            {
                response.IsSuccess= false;
                response.Data = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> Create(MerchantCreateDTO merchantFromReq)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
            }
            try
            {
                var newUser = mapper.Map<ApplicationUser>(merchantFromReq);
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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> EditById(int id, [FromBody]MerchantEditDTO merchantFromReq)
        {
            if(!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
            }
            try
            {
                var merchant = await service.GetByIdAsync(id);

                if(merchant == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Merchant not found.";
                    return response;
                }

                if (merchant.ApplicationUser == null)
                {
                    merchant.ApplicationUser = new ApplicationUser();
                }

                merchant.StoreName = merchantFromReq.StoreName;
                merchant.Government = merchantFromReq.Government;
                merchant.City = merchantFromReq.City;
                merchant.PickupCost = merchantFromReq.PickupCost;
                merchant.RejectedOrderPercentage = merchantFromReq.RejectedOrderPercentage;

                merchant.ApplicationUser.UserName = merchantFromReq.Name;
                merchant.ApplicationUser.Email = merchantFromReq.Email;
                merchant.ApplicationUser.PhoneNumber = merchantFromReq.Phone;
                merchant.ApplicationUser.Address = merchantFromReq.Address;

                if (!string.IsNullOrWhiteSpace(merchantFromReq.CurrentPassword) &&
                    !string.IsNullOrWhiteSpace(merchantFromReq.NewPassword))
                {
                    var user = await userManager.FindByIdAsync(merchant.ApplicationUser.Id.ToString());
                            
                    if (user != null)
                    {
                        var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, merchantFromReq.CurrentPassword);
                        if (!isCurrentPasswordValid)
                        {
                            response.IsSuccess = false;
                            response.Data = "Current password is incorrect.";
                            return response;
                        }

                        var passwordResult = await userManager.ChangePasswordAsync(user, merchantFromReq.CurrentPassword, merchantFromReq.NewPassword);
                        if (!passwordResult.Succeeded)
                        {
                            response.IsSuccess = false;
                            response.Data = passwordResult.Errors.Select(e => e.Description).ToList();
                            return response;
                        }
                    }
                }

                await service.UpdateAsync(merchant);
                await service.SaveChangesAsync();
                response.IsSuccess = true;
                response.Data = "Merchant updated successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> DeleteById(int id)
        {
            try
            {
                await service.DeleteAsync(id);
                await service.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Merchant deleted successfully.";
            }
            catch (KeyNotFoundException ex) // لو انا دخلت id مش موجود
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }
            catch (InvalidOperationException ex) // id موجود بس انا كنت عامله soft delete
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }
            catch (Exception ex) // بيهندل اي exception جاي من ال server
            {
                response.IsSuccess = false; 
                response.Data = ex.Message;
            }

            return response;
        }
    }
}
