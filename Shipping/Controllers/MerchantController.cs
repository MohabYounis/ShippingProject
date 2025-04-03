using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        UserManager<ApplicationUser> userManager;
        RoleManager<ApplicationRole> roleManager;
        public MerchantController(IServiceGeneric<Merchant> service, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMerchantService merchantService)
        {
            this.service = service;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
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
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (merchants == null || !merchants.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
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

                        if (!merchants.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
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

                    return Ok(GeneralResponse.Success(result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> GetById(int id)
        {
            try
            {
                var merchant = await service.GetByIdAsync(id);
                MerchantGetDTO merchantDTO = mapper.Map<MerchantGetDTO>(merchant);
                return Ok(GeneralResponse.Success(merchantDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> Create(MerchantCreateDTO merchantFromReq)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            try
            {
                bool roleExists = await roleManager.RoleExistsAsync("merchant");
                if (!roleExists) throw new Exception("Role 'merchant' does not exist.");

                var newUser = mapper.Map<ApplicationUser>(merchantFromReq);
                var result = await userManager.CreateAsync(newUser, merchantFromReq.Password);
                await userManager.AddToRoleAsync(newUser, "merchant");

                if (result.Succeeded) return Ok(GeneralResponse.Success("Merchant Created Successfully"));
                else
                {
                    string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return BadRequest(GeneralResponse.Failure(errors));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> EditById(int id, [FromBody]MerchantEditDTO merchantFromReq)
        {
            if(!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            try
            {
                var merchant = await service.GetByIdAsync(id);

                if(merchant == null) return NotFound(GeneralResponse.Failure("Merchant not found."));

                merchant.ApplicationUser ??= new ApplicationUser();
                merchant.SpecialShippingRates ??= new List<SpecialShippingRate>();
                merchant.BranchMerchants ??= merchant.BranchMerchants = new List<BranchMerchant>();
                
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
                        if (!isCurrentPasswordValid) return BadRequest(GeneralResponse.Failure("Current password is incorrect."));

                        var passwordResult = await userManager.ChangePasswordAsync(user, merchantFromReq.CurrentPassword, merchantFromReq.NewPassword);
                        if (!passwordResult.Succeeded)
                        {
                            string errors = string.Join("; ", passwordResult.Errors.Select(e => e.Description));
                            return BadRequest(GeneralResponse.Failure(errors));
                        }
                    }
                }

                await service.UpdateAsync(merchant);
                await service.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Merchant updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> DeleteById(int id)
        {
            try
            {
                await service.DeleteAsync(id);
                await service.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Merchant deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
    }
}
