using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.NewFolder1;
using Shipping.DTOs.SpecialShippingRatesDTOs;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        IServiceGeneric<Merchant> service;
        IMerchantService merchantService;
        IMapper mapper;
        GeneralResponse response;
        UserManager<ApplicationUser> userManager;
        RoleManager<ApplicationRole> roleManager;
        public MerchantController(IServiceGeneric<Merchant> service, IMapper mapper, GeneralResponse response, UserManager<ApplicationUser> userManager, IMerchantService merchantService)
        {
            this.service = service;
            this.mapper = mapper;
            this.response = response;
            this.userManager = userManager;
            this.merchantService = merchantService;
        }


        [HttpGet("{all:alpha}")]
        public async Task<ActionResult<GeneralResponse>> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Merchant> merchants;
                if (all == "all") merchants = await merchantService.GetAllAsync();
                else if (all == "exist") merchants = await merchantService.GetAllExistAsync();
                else
                {
                    response.IsSuccess = false;
                    response.Data = "Parameter Not Exist";
                    return BadRequest(response);
                }

                if (merchants == null || !merchants.Any())
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                    return NotFound(response);
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching by name or phone
                        merchants = merchants
                            .Where(item =>
                                (item.ApplicationUser.UserName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ApplicationUser.PhoneNumber?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!merchants.Any())
                        {
                            response.IsSuccess = false;
                            response.Data = "No Found";
                            return NotFound(response);
                        }
                    }

                    var totalMerchnts = merchants.Count();

                    // Pagination
                    var paginatedMerchants = merchants
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    List<MerchantGetDTO> merchantDTO = mapper.Map<List<MerchantGetDTO>>(paginatedMerchants);

                    
                    var result = new
                    {
                        TotalMerchants = totalMerchnts,       // العدد الإجمالي للعناصر
                        Page = page,                          // الصفحة الحالية
                        PageSize = pageSize,                  // عدد العناصر في الصفحة
                        Merchants = merchantDTO               // العناصر الحالية
                    };

                    response.IsSuccess = true;
                    response.Data = result;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess= false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
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
                return BadRequest(response);
            }
            try
            {
                var newUser = mapper.Map<ApplicationUser>(merchantFromReq);

                bool roleExists = await roleManager.RoleExistsAsync("merchant");
                if (!roleExists) throw new Exception("Role 'merchant' does not exist.");
                await userManager.AddToRoleAsync(newUser, "merchant");

                var result = await userManager.CreateAsync(newUser, merchantFromReq.Password);
                if (result.Succeeded)
                {
                    response.IsSuccess = true;
                    response.Data = "Merchant Created Successfully";
                    return CreatedAtAction("Create", response);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Data = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
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
                return BadRequest(response);
            }
            try
            {
                var merchant = await service.GetByIdAsync(id);

                if(merchant == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Merchant not found.";
                    return NotFound(response);
                }

                if (merchant.ApplicationUser == null)
                {
                    merchant.ApplicationUser = new ApplicationUser();
                }

                if (merchant.SpecialShippingRates == null)
                {
                    merchant.SpecialShippingRates = new List<SpecialShippingRate>();
                }
                
                if (merchant.BranchMerchants == null)
                {
                    merchant.BranchMerchants = new List<BranchMerchant>();
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

                // تحديث العناصر القديمة أو إضافة الجديدة
                var existingRates = merchant.SpecialShippingRates.ToDictionary(s => s.City_Id);
                var newRates = merchantFromReq.SpecialShippingRates ?? new List<SpecialCreateDTO>();
                foreach (var newRate in newRates)
                {
                    if (existingRates.TryGetValue(newRate.City_Id, out var existingRate))
                    {
                        existingRate.SpecialPrice = newRate.SpecialPrice; // تحديث البيانات
                    }
                    else
                    {
                        merchant.SpecialShippingRates.Add(new SpecialShippingRate
                        {
                            City_Id = newRate.City_Id,
                            SpecialPrice = newRate.SpecialPrice
                        });
                    }
                }
                // حذف الأسعار الخاصة التي لم تعد موجودة
                merchant.SpecialShippingRates.RemoveAll(r => !newRates.Any(n => n.City_Id == r.City_Id));


                var existingBranches = merchant.BranchMerchants.ToDictionary(b => b.Branch_Id);
                var newBranches = merchantFromReq.Branches_Id ?? new List<int>();

                foreach (var newBranch in newBranches)
                {
                    if (!existingBranches.ContainsKey(newBranch))
                    {
                        // إضافة فرع جديد
                        merchant.BranchMerchants.Add(new BranchMerchant
                        {
                            Merchant_Id = id,
                            Branch_Id = newBranch
                        });
                    }
                }
                // حذف الفروع التي لم تعد موجودة في الطلب
                merchant.BranchMerchants.RemoveAll(b => !newBranches.Contains(b.Branch_Id));

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
                            return BadRequest(response);
                        }

                        var passwordResult = await userManager.ChangePasswordAsync(user, merchantFromReq.CurrentPassword, merchantFromReq.NewPassword);
                        if (!passwordResult.Succeeded)
                        {
                            response.IsSuccess = false;
                            response.Data = passwordResult.Errors.Select(e => e.Description).ToList();
                            return BadRequest(response);
                        }
                    }
                }

                await service.UpdateAsync(merchant);
                await service.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Merchant updated successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false; 
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
