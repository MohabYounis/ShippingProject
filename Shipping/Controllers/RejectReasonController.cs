
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rejectReasons = await _rejectReasonService.GetAllAsync();
            return Ok(rejectReasons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rejectReason = await _rejectReasonService.GetByIdAsync(id);
            if (rejectReason == null) return NotFound();
            return Ok(rejectReason);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RejectReasonDTO rejectReasonDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _rejectReasonService.AddAsync(rejectReasonDto);
            return CreatedAtAction(nameof(GetById), new { id = rejectReasonDto.Id }, rejectReasonDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RejectReasonDTO rejectReasonDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _rejectReasonService.UpdateAsync(id, rejectReasonDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rejectReasonService.DeleteAsync(id);
            return NoContent();
        }
    }
}