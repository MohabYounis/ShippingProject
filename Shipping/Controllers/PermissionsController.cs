using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs.Permissions;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {


        IServiceGeneric<Permission> permissionService;

        public PermissionsController(IServiceGeneric<Permission> permissionService)
        {
            this.permissionService = permissionService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAll()
        {
            //geeting from db
            var permissions = await permissionService.GetAllAsync();
            if (permissions == null || !permissions.Any()) { return NotFound(); }

            //mapping
            var permisionsDTO = permissions.Select(p => new PermissionsDTO
            {
                Id = p.Id,
                Name = p.Name,
                IsDeleted = p.IsDeleted,
            });



            return Ok(permisionsDTO);

        }

        [HttpGet("Exist")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAllExist()
        {
            //geeting from db
            var permissions = await permissionService.GetAllExistAsync();
            if (permissions == null || !permissions.Any()) { return NotFound(); }

            //mapping
            var permisionsDTO = permissions.Select(p => new PermissionsDTO
            {
                Id = p.Id,
                Name = p.Name,
                IsDeleted = p.IsDeleted,
            });



            return Ok(permisionsDTO);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermission(int id)
        {
            if (id <= 0) return BadRequest($"{id} is a not valid id");
            var permission = await permissionService.GetByIdAsync(id);

            if (permission == null) return NotFound($"there is no any Permissions with {id} id");

            //mapping
            var permissionDTO = new PermissionsDTO
            {
                Id = permission.Id,
                Name = permission.Name,
                IsDeleted = permission.IsDeleted,
            };
            return Ok(permissionDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Permission>> AddPermission([FromBody] CreatePermissionsDTO permissionDTO)
        {
            //
            if (!ModelState.IsValid) return BadRequest("Invalid data.");
            try
            {
                //mapping
                var permission = new Permission
                {
                    Name = permissionDTO.Name,
                };
                //adding to db

                await permissionService.AddAsync(permission);
                await permissionService.SaveChangesAsync();
                return Ok(permissionDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "error while saving in DataBase");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermission(int id, [FromBody] CreatePermissionsDTO permissionDTO)
        {
            if (id <= 0) return BadRequest("Invalid ID");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var permission = await permissionService.GetByIdAsync(id);
                if (permission == null) return NotFound("there is no any Permissions with this id");

                //mapping
                permission.Name = permissionDTO.Name;

                //adding to db
                await permissionService.UpdateAsync(permission);
                await permissionService.SaveChangesAsync();

                return Ok("updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "error while saving in DataBase");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            try
            {
            
                await permissionService.DeleteAsync(id);
                await permissionService.SaveChangesAsync();
                return Ok("Deleted successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500,"an error occurs while  deleting");

            }
        }
    }
}



