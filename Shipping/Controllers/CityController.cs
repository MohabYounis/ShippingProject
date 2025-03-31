using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Shipping.DTOs;
using Shipping.DTOs.CityDTOs;
using Shipping.DTOs.GovernmentDTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.ImodelRepository;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        IServiceGeneric<City> serviceCityGernric;
        private readonly IMapper mapper;
        private readonly ICityService cityService;

        public CityController(IServiceGeneric<City> serviceCityGernric, IMapper mapper, ICityService cityService)
        {
            this.serviceCityGernric = serviceCityGernric;
            this.mapper = mapper;
            this.cityService = cityService;
        }


        [HttpGet("{all:alpha}")]
        public async Task<ActionResult<GeneralResponse>> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<City> cities;
                if (all == "all") cities = await cityService.GetAllAsync();
                else if (all == "exist") cities = await cityService.GetAllExistAsync();
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (cities == null || !cities.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching by name or phone
                        cities = cities
                            .Where(item =>
                                (item.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.Government.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!cities.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    var totalCities = cities.Count();

                    // Pagination
                    var paginatedCities = cities
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var citiesDTO = mapper.Map<List<CityGetDTO>>(paginatedCities);


                    var result = new
                    {
                        TotalCitiess = totalCities,             // العدد الإجمالي للعناصر
                        Page = page,                            // الصفحة الحالية
                        PageSize = pageSize,                    // عدد العناصر في الصفحة
                        Merchants = citiesDTO                   // العناصر الحالية
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
                var city = await serviceCityGernric.GetByIdAsync(id);
                if (city == null) return NotFound(GeneralResponse.Failure("Not Found."));
                
                var cityDTO = mapper.Map<CityGetDTO>(city);
                return Ok(GeneralResponse.Success(cityDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> Create(CityCreateDTO cityFromReq)
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
                var cityByName = await cityService.GetByNameAsync(cityFromReq.Name);
                if (cityByName != null) return BadRequest(GeneralResponse.Failure("City is already exist."));
                
                var city = mapper.Map<City>(cityFromReq);
                await serviceCityGernric.AddAsync(city);
                await serviceCityGernric.SaveChangesAsync();
                return Ok(GeneralResponse.Success("City created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> EditById(int id, [FromBody] CityEditDTO cityFromReq)
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
                var city = mapper.Map<City>(cityFromReq);
                city.Id = id;
                await serviceCityGernric.UpdateAsync(city);
                await serviceCityGernric.SaveChangesAsync();
                return Ok(GeneralResponse.Success("City updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> Delete(int id)
        {
            try
            {
                await serviceCityGernric.DeleteAsync(id);
                await serviceCityGernric.SaveChangesAsync();
                return Ok(GeneralResponse.Success("City deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
    }
}
