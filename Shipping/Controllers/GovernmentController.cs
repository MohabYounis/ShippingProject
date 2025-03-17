using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Services;

namespace Shipping.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GovernmentController : ControllerBase
    {
        private readonly IGovernmentService _governmentService;

        public GovernmentController(IGovernmentService governmentService)
        {
            _governmentService = governmentService;
        }

        // Get All Governments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var governments = await _governmentService.GetAllGovernmentsAsync();
            return Ok(governments);
        }

        // Get Government by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var government = await _governmentService.GetGovernmentByIdAsync(id);
            if (government == null)
                return NotFound();

            return Ok(government);
        }

        // Add Government
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Government government)
        {
            if (government == null)
                return BadRequest();

            await _governmentService.AddGovernmentAsync(government);
            return CreatedAtAction(nameof(GetById), new { id = government.Id }, government);
        }

        // Update Government
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Government government)
        {
            if (government == null || id != government.Id)
                return BadRequest();

            await _governmentService.UpdateGovernmentAsync(government);
            return NoContent();
        }

        // Delete Government (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _governmentService.DeleteGovernmentAsync(id);
            return NoContent();
        }
    }
}