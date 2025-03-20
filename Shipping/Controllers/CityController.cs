using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Shipping.DTOs;
using Shipping.DTOs.CityDTOs;
using Shipping.ImodelRepository;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        IServiceGeneric<City> serviceCityGernric;
        GeneralResponse response;
        private readonly IMapper mapper;
        private readonly ICityService cityService;

        public CityController(IServiceGeneric<City> serviceCityGernric, GeneralResponse response, IMapper mapper, ICityService cityService)
        {
            this.serviceCityGernric = serviceCityGernric;
            this.response = response;
            this.mapper = mapper;
            this.cityService = cityService;
        }


        [HttpGet("All")]
        public async Task <ActionResult<GeneralResponse>> GetAll()
        {
            try
            {
                var cities = await cityService.GetAllAsync();
                var citiesDTO = mapper.Map<List<CityGetDTO>>(cities);
                if (cities == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Not Found";
                    return NotFound(response);
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = citiesDTO;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess= false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpGet("AllExist")]
        public async Task<ActionResult<GeneralResponse>> GetAllExist()
        {
            try
            {
                var cities = await cityService.GetAllExistAsync();
                var citiesDTO = mapper.Map<List<CityGetDTO>>(cities);

                if (cities == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Not Found";
                    return NotFound(response);
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = citiesDTO;
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
        public async Task <ActionResult<GeneralResponse>> GetById(int id)
        {
            try
            {
                var city = await serviceCityGernric.GetByIdAsync(id);
                var cityDTO = mapper.Map <CityGetDTO>(city);

                if (city == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Not Found";
                    return NotFound(response);
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = cityDTO;
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


        [HttpPost]
        public async Task <ActionResult<GeneralResponse>> Create(CityCreateDTO cityFromReq)
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
                var city = mapper.Map<City>(cityFromReq);
                await serviceCityGernric.AddAsync(city);
                await serviceCityGernric.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "City created successfully.";
                return CreatedAtAction("Create", response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> EditById(int id, [FromBody] CityEditDTO cityFromReq)
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
                var city = mapper.Map<City>(cityFromReq);
                city.Id = id;
                await serviceCityGernric.UpdateAsync(city);
                await serviceCityGernric.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "City updated successfully.";
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
        public async Task <ActionResult<GeneralResponse>> Delete (int id)
        {
            try
            {
                await serviceCityGernric.DeleteAsync(id);
                await serviceCityGernric.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "City deleted successfully.";
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
