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
        public SettingController(IServiceGeneric<Setting> settingService)
        {
          this.settingService = settingService;
        }

        // Get All setting
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var settings = await settingService.GetAllAsync();
            var settingsDtos = settings.Select(s => new SettingDTO
            {
                Id = s.Id,
                ShippingToVillageCost =s.ShippingToVillageCost,
                DeliveryAutoAccept=s.DeliveryAutoAccept,
                IsDeleted = s.IsDeleted,
            }).ToList();

            return Ok(settingsDtos);
        }

        // Get setting by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            GeneralResponse response = new GeneralResponse();

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
            catch (KeyNotFoundException)
            {
                response.IsSuccess = false;
                response.Data = $"Setting with ID {id} was not found.";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);


            //var setting = await settingService.GetByIdAsync(id);
            //if (setting == null)
            //    return NotFound(new { message = $"Setting with ID {id} was not found." });
            //var settingDto = new SettingDTO
            //{
            //    Id = setting.Id,
            //    ShippingToVillageCost = setting.ShippingToVillageCost,
            //    DeliveryAutoAccept = setting.DeliveryAutoAccept,
            //    IsDeleted = setting.IsDeleted
            //};

            //return Ok(settingDto);
        }

        //Add setting
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SettingCreateDTOS settingcreateDto)
        {
            if (settingcreateDto == null)
                return BadRequest("The data is incorrect");

          
            var existingSetting = await settingService.GetAllAsync();
            if (existingSetting.Any())
                return BadRequest("A Setting has already been created. You cannot add another one.");

          
            var setting = new Setting
            {
                ShippingToVillageCost = settingcreateDto.ShippingToVillageCost ,
                DeliveryAutoAccept = settingcreateDto.DeliveryAutoAccept ,
                IsDeleted = false
            };

            await settingService.AddAsync(setting);
            await settingService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = setting.Id }, setting);
        }

        // Update setting by ID 
        [HttpPut("{id}")]
        public async Task<IActionResult> EditById(int id, [FromBody] SettingEditDTO settingUpdateDto)
        {
            GeneralResponse response = new GeneralResponse();

            if (settingUpdateDto == null)
            {
                response.IsSuccess = false;
                response.Data = "The data is incorrect.";
                return BadRequest(response);
            }

            try
            {
                var setting = await settingService.GetByIdAsync(id);
                if (setting == null)
                {
                    response.IsSuccess = false;
                    response.Data = $"Setting with ID {id} was not found.";
                    return NotFound(response);
                }

                setting.Id = settingUpdateDto.Id;   
                setting.ShippingToVillageCost = settingUpdateDto.ShippingToVillageCost;
                setting.DeliveryAutoAccept = settingUpdateDto.DeliveryAutoAccept;
                
                await settingService.UpdateAsync(id);
                await settingService.SaveChangesAsync();

                var updatedSettingDto = new SettingEditDTO
                {
                    Id = setting.Id,
                    ShippingToVillageCost = setting.ShippingToVillageCost,
                    DeliveryAutoAccept = setting.DeliveryAutoAccept,  
                };
                response.IsSuccess = true;
                response.Data = updatedSettingDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }
    }
}

