using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.Models;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IApplicationRoleService _roleService;
        private readonly IMapper mapper;

        public RoleController(IApplicationRoleService roleService , IMapper mapper)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            this.mapper = mapper;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                return Ok(role);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] ApplicationRoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _roleService.AddAsync(role);
             _roleService.SaveDB();

            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] ApplicationRoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingRole = await _roleService.GetByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }
                role.Id = id; 
                _roleService.Update(role);
                _roleService.SaveDB();  
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var existingRole = await _roleService.GetByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }
                await _roleService.Delete(id);
                _roleService.SaveDB();

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
        }

    }
}
