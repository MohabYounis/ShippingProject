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
        IServiceGeneric<City> service;
        GeneralResponse response;
        private readonly IMapper mapper;
        private readonly ICityService cityService;

        public CityController(IServiceGeneric<City> service, GeneralResponse response, IMapper mapper, ICityService cityService)
        {
            this.service = service;
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
                var city = await service.GetByIdAsync(id);
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

    }
}
