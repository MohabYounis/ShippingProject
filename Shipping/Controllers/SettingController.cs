using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.Branch;
using Shipping.DTOs.setting;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IServiceGeneric<Setting> settingService;

        public SettingController(IServiceGeneric<Setting> settingService)
        {
            this.settingService = settingService;
        }


        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var settings = await settingService.GetAllAsync();
                var settingsDtos = settings.Select(s => new SettingDTO
                {
                    Id = s.Id,
                    ShippingToVillageCost = s.ShippingToVillageCost,
                    DeliveryAutoAccept = s.DeliveryAutoAccept,
                    IsDeleted = s.IsDeleted,
                }).ToList();

                return Ok(GeneralResponse.Success(settingsDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
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

                return Ok(GeneralResponse.Success(settingDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SettingCreateDTOS settingcreateDto)
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
                var existingSetting = await settingService.GetAllAsync();
                if (existingSetting.Any()) return BadRequest(GeneralResponse.Failure("A Setting has already been created. You cannot add another one."));

                var setting = new Setting
                {
                    ShippingToVillageCost = settingcreateDto.ShippingToVillageCost,
                };

                await settingService.AddAsync(setting);
                await settingService.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Setting created successfully."));

            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditById(int id, [FromBody] SettingEditDTO settingUpdateDto)
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
                var setting = await settingService.GetByIdAsync(id);
                if (setting == null) return NotFound(GeneralResponse.Failure($"Setting with ID {id} was not found."));

                setting.ShippingToVillageCost = settingUpdateDto.ShippingToVillageCost;
                setting.DeliveryAutoAccept = settingUpdateDto.DeliveryAutoAccept;

                await settingService.UpdateAsync(setting);
                await settingService.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Setting updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));

            }
        }
    }
}

