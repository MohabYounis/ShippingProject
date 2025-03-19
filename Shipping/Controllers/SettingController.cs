using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.Branch;
using Shipping.DTOs.setting;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IServiceGeneric<Setting> settingService;
        private readonly GeneralResponse response;

        public SettingController(IServiceGeneric<Setting> settingService, GeneralResponse response)
        {
            this.settingService = settingService;
            this.response = response;
        }


        // Get All setting
        [HttpGet]
        public async Task<ActionResult<GeneralResponse>> GetAll()
        {
            try
            {
                var settings = await settingService.GetAllAsync();
                var settingsDtos = settings.Select(s => new SettingDTO
                {
                    Id = s.Id,
                    ShippingToVillageCost =s.ShippingToVillageCost,
                    DeliveryAutoAccept=s.DeliveryAutoAccept,
                    IsDeleted = s.IsDeleted,
                }).ToList();

                response.IsSuccess = true;
                response.Data = settingsDtos;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }

        // Get setting by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> GetById(int id)
        {
            try
            {
                var setting = await settingService.GetByIdAsync(id);
                var settingDto = new SettingDTO
                {
                    Id = setting.Id,
                    ShippingToVillageCost = setting.ShippingToVillageCost,
                    DeliveryAutoAccept = setting.DeliveryAutoAccept,
                    IsDeleted = setting.IsDeleted
                };

                response.IsSuccess = true;
                response.Data = settingDto;
            }
            catch (KeyNotFoundException ex)
            {
                response.IsSuccess = false;
                response.Data = $"Setting with ID {id} was not found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }


        //Add setting
        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> Create([FromBody] SettingCreateDTOS settingcreateDto)
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
                var existingSetting = await settingService.GetAllAsync();
                if (existingSetting.Any())
                {
                    response.IsSuccess = false;
                    response.Data = "A Setting has already been created. You cannot add another one.";
                    return response;
                }

                var setting = new Setting
                {
                    ShippingToVillageCost = settingcreateDto.ShippingToVillageCost,
                };

                await settingService.AddAsync(setting);
                await settingService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Setting created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }
            
            return response;
        }

        // Update setting by ID 
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> EditById(int id, [FromBody] SettingEditDTO settingUpdateDto)
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
                var setting = await settingService.GetByIdAsync(id);
                if (setting == null)
                {
                    response.IsSuccess = false;
                    response.Data = $"Setting with ID {id} was not found.";
                }

                setting.ShippingToVillageCost = settingUpdateDto.ShippingToVillageCost;
                setting.DeliveryAutoAccept = settingUpdateDto.DeliveryAutoAccept;
                
                await settingService.UpdateAsync(id);
                await settingService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Setting updated successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }
    }
}

