
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.GovernmentDTOs;
using Shipping.DTOs.RejectedReasonsDTOs;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RejectReasonController : ControllerBase
    {
        private readonly IRejectReasonService _rejectReasonService;

        public RejectReasonController(IRejectReasonService rejectReasonService)
        {
            _rejectReasonService = rejectReasonService;
        }


        [HttpGet("{all:alpha}")]
        public async Task<IActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<RejectReasonDTO> rejectDTO;
                if (all == "all") rejectDTO = await _rejectReasonService.GetAllAsync();
                else if (all == "exist") rejectDTO = await _rejectReasonService.GetAllExistAsync();
                else
                {
                    return BadRequest();
                }

                if (rejectDTO == null || !rejectDTO.Any())
                {
                    return NotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching
                        rejectDTO = rejectDTO
                            .Where(item =>
                                (item.Reason?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                        )
                        .ToList();

                        if (!rejectDTO.Any())
                        {
                            return NotFound();
                        }
                    }

                    var totalReasons = rejectDTO.Count();

                    // Pagination
                    var paginatedReasons = rejectDTO
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                        .ToList();

                    var result = new
                    {
                        TotalReasons = totalReasons,                    // العدد الإجمالي للعناصر
                        Page = page,                                    // الصفحة الحالية
                        PageSize = pageSize,                            // عدد العناصر في الصفحة
                        Reasons = paginatedReasons                      // العناصر الحالية
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
            var rejectReason = await _rejectReasonService.GetByIdAsync(id);
            if (rejectReason == null) return NotFound();
            return Ok(rejectReason);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RejectReasonCreateDTO rejectReasonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()));
            }
            try
            {
                await _rejectReasonService.AddAsync(rejectReasonDto);
                return Ok(new { message = "RejectReason added successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RejectReasonDTO rejectReasonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()));
            }
            try
            {
                await _rejectReasonService.UpdateAsync(id, rejectReasonDto);
                return Ok(new { message = "RejectReason updated successfully!" });
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
                await _rejectReasonService.DeleteAsync(id);
                return Ok(new { message = "RejectReason deleted successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}