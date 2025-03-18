
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
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
                return NotFound("Government not found.");
            return Ok(government);
        }

        // Add a new Government
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GovernmentDTO governmentDto)
        {
            if (governmentDto == null)
                return BadRequest("Invalid data.");

            await _governmentService.AddGovernmentAsync(governmentDto);
            return Ok(new { message = "Government added successfully!" });
        }

        // Update an existing Government
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GovernmentDTO governmentDto)
        {
            try
            {
                await _governmentService.UpdateGovernmentAsync(id, governmentDto);
                return Ok(new { message = "Government updated successfully!" });
            }
            catch (Exception)
            {
                return NotFound("Government not found.");
            }
        }

        // Delete a Government
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _governmentService.DeleteGovernmentAsync(id);
                return Ok(new { message = "Government deleted successfully!" });
            }
            catch (Exception)
            {
                return NotFound("Government not found.");
            }
        }
    }
}