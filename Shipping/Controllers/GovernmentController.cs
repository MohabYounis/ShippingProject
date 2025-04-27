using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.GovernmentDTOs;
using Shipping.Models;
using Shipping.Services;

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

        /// <summary>
        /// Retrieves a list of governments with optional search and pagination.
        /// </summary>
        /// <param name="searchTxt">Search keyword to filter by government name (optional).</param>
        /// <param name="all">Use "all" to retrieve all governments, "exist" to retrieve only non-deleted ones.</param>
        /// <param name="page">Page number (default is 1).</param>
        /// <param name="pageSize">Number of items per page (default is 10).</param>
        /// <returns>
        /// 200 OK with list of governments,  
        /// 404 Not Found if no items found,  
        /// 400 BadRequest if parameter 'all' is invalid.
        /// </returns>
        [HttpGet("{all:alpha}")]
        public async Task<IActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<GovernmentGetDTO> governments;
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
                                (item.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.BranchName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
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

        /// <summary>
        /// Retrieves a specific government by its ID.
        /// </summary>
        /// <param name="id">The government ID.</param>
        /// <returns>
        /// 200 OK with government data,  
        /// 404 Not Found if not found.
        /// </returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var government = await _governmentService.GetGovernmentByIdAsync(id);
            if (government == null) return NotFound();
            return Ok(government);
        }

        /// <summary>
        /// Adds a new government.
        /// </summary>
        /// <param name="governmentDto">The government data to be added.</param>
        /// <returns>
        /// 200 OK if added successfully,  
        /// 400 BadRequest if data is invalid or already exists,  
        /// 500 Internal Server Error if something went wrong.
        /// </returns>
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

        /// <summary>
        /// Updates an existing government by ID.
        /// </summary>
        /// <param name="id">The ID of the government to update.</param>
        /// <param name="governmentDto">The updated government data.</param>
        /// <returns>
        /// 200 OK if updated successfully,  
        /// 400 BadRequest if data is invalid,  
        /// 500 Internal Server Error on failure.
        /// </returns>
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

        /// <summary>
        /// Deletes a government by ID.
        /// </summary>
        /// <param name="id">The ID of the government to delete.</param>
        /// <returns>
        /// 200 OK if deleted successfully,  
        /// 500 Internal Server Error on failure.
        /// </returns>
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