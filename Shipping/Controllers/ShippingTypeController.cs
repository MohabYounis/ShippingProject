using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.ShippingType;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.ModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingTypeController : ControllerBase
    {
        IServiceGeneric<ShippingType> shippingService;
        IServiceGeneric<Order> orderService;

        public ShippingTypeController(IServiceGeneric<ShippingType> shippingService, IServiceGeneric<Order> orderService)
        {
            this.shippingService = shippingService;
            this.orderService = orderService;
        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAllShipping()
        {
            //getting all shipping types

            var shippingTypes = await shippingService.GetAllAsync();

            //if there is no shipping types

            if (shippingTypes == null || !shippingTypes.Any())
            {
                return NotFound("there is no shipping types ");
            }

            //mapping shippingDTO
            var shippingTypeDtos = shippingTypes.Select(s => new ShippingTypeDTO
            {
                Id = s.Id,
                Type = s.Type,
                Description = s.Description,
                Cost = s.Cost,
                IsDeleted = s.IsDeleted
            }).ToList();

            return Ok(shippingTypeDtos);
        }


        [HttpGet("exist")]
        public async Task<IActionResult> GetAllExistShipping()
        {
            //getting existing

            var shippingTypes = await shippingService.GetAllExistAsync();

            //check
            if (shippingTypes == null || !shippingTypes.Any())
            {
                return NotFound("there is no shipping types ");
            }

            //  mapping
            var ShippingTypeDtos = shippingTypes.Select(s => new ShippingTypeDTO
            {
                Id = s.Id,
                Type = s.Type,
                Description = s.Description,
                Cost = s.Cost,
                IsDeleted = s.IsDeleted
            }).ToList();

            return Ok(ShippingTypeDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipping(int id)
        {
            if (id <= 0)
            {
                return BadRequest("id must be greater than 0");
            }
            //getting from db
            var shippingType = await shippingService.GetByIdAsync(id);
            //check
            if (shippingType == null)
            {
                return NotFound("there is no shipping type with this id");
            }

            //mapping dto
            var shippingTypeDto = new ShippingTypeDTO
            {
                Id = shippingType.Id,
                Type = shippingType.Type,
                Description = shippingType.Description,
                Cost = shippingType.Cost,
                IsDeleted = shippingType.IsDeleted
            };

            return Ok(shippingTypeDto);
        }



        [HttpPost]
        public async Task<IActionResult> AddShipping([FromBody] CreateShippingTypeDTO ShippingDto)
        {
            //check
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //mapping
            var shippingType = new ShippingType
            {
                Type = ShippingDto.Type,
                Description = ShippingDto.Description,
                Cost = ShippingDto.Cost,
            };
            try
            {
                //adding to db
                await shippingService.AddAsync(shippingType);
                await shippingService.SaveChangesAsync();

                return Ok("Shipping added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipping(int id, [FromBody] ShippingTypeDTO ShippingDto)
        {
            //check id 
            if (id <= 0) return BadRequest("id must be greater than 0");

            //check 
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //getting from db

            var ShippingType = await shippingService.GetByIdAsync(id);
            if (ShippingType == null)
            {
                return NotFound($"there is no shipping type with this id {id}");
            }

            //mapping 

            ShippingType.Type = ShippingDto.Type;
            ShippingType.Description = ShippingDto.Description;
            ShippingType.Cost = ShippingDto.Cost;

            await shippingService.UpdateAsync(ShippingType);
            await shippingService.SaveChangesAsync();

            return Ok("Shipping updated successfully");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipping(int id)
        {
            //check 
            if (id <= 0)
            {
                return BadRequest("id must be greater than 0");
            }
            //getting from db
            var shippingType = await shippingService.GetByIdAsync(id);

            if (shippingType == null)
            {
                return NotFound($"there is no shipping type with this id {id}");
            }
            if (shippingType.IsDeleted)
            {
                return BadRequest("Shipping already deleted");
            }
            await shippingService.DeleteAsync(id);
            await shippingService.SaveChangesAsync();

            return Ok("Shipping deleted successfully");
        }
    }
}
