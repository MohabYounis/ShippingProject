
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.GovernmentDTOs;
using Shipping.DTOs.MerchantDTOs;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.ModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernmentController : ControllerBase
    {
        private readonly IGovernmentService _governmentService;
        private readonly  IServiceGeneric<Government> genericService;

        public GovernmentController(IGovernmentService governmentService, IServiceGeneric<Government> genericService)
        {
            _governmentService = governmentService;
            this.genericService = genericService;
        }


        [HttpGet("{all:alpha}")]
        public async Task<IActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<GovernmentDTO> governments;
                if (all == "all") governments = await _governmentService.GetAllGovernmentsAsync();
                else if (all == "exist") governments = await _governmentService.GetAllExistGovernmentsAsync();
                else
                {
                    return BadRequest();
                }

                if (governments == null || !governments.Any())
                {
                    return NotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching
                        governments = governments
                            .Where(item =>
                                (item.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                        )
                        .ToList();

                        if (!governments.Any())
                        {
                            return NotFound();
                        }
                    }

                    var totalGovernments = governments.Count();

                    // Pagination
                    var paginatedGovernments = governments
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                        .ToList();

                    var result = new
                    {
                        TotalGovernments = totalGovernments,            // العدد الإجمالي للعناصر
                        Page = page,                                    // الصفحة الحالية
                        PageSize = pageSize,                            // عدد العناصر في الصفحة
                        Governments = paginatedGovernments              // العناصر الحالية
                    };

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var government = await _governmentService.GetGovernmentByIdAsync(id);
            if (government == null) return NotFound();
            return Ok(government);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GovernmentCreateDTO governmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()));
            }
            try
            {
                var gov = await genericService.GetByNameAsync(governmentDto.Name);
                if (gov != null)
                {
                    return BadRequest("Governorate is already exist.");
                }

                await _governmentService.AddGovernmentAsync(governmentDto);
                return Ok(new { message = "Government added successfully!" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GovernmentDTO governmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()));
            }
            try
            {
                await _governmentService.UpdateGovernmentAsync(id, governmentDto);
                return Ok(new { message = "Government updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _governmentService.DeleteGovernmentAsync(id);
                return Ok(new { message = "Government deleted successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}