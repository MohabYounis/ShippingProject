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

        /// <summary>
        /// Retrieves all permissions from the database.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of all permissions,  
        /// 404 NotFound if no permissions are found.
        /// </returns>
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

        /// <summary>
        /// Retrieves all existing (non-deleted) permissions from the database.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of existing permissions,  
        /// 404 NotFound if no existing permissions are found.
        /// </returns>
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

        /// <summary>
        /// Retrieves a specific permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission to retrieve.</param>
        /// <returns>
        /// 200 OK with the permission details,  
        /// 400 BadRequest if the ID is invalid,  
        /// 404 NotFound if no permission is found with the provided ID.
        /// </returns>
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

        /// <summary>
        /// Adds a new permission to the database.
        /// </summary>
        /// <param name="permissionDTO">The permission data to be added.</param>
        /// <returns>
        /// 200 OK if the permission is added successfully,  
        /// 400 BadRequest if the input data is invalid,  
        /// 500 InternalServerError if an error occurs during the save process.
        /// </returns>
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

        /// <summary>
        /// Updates an existing permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission to update.</param>
        /// <param name="permissionDTO">The updated permission data.</param>
        /// <returns>
        /// 200 OK if the update is successful,  
        /// 400 BadRequest if the ID is invalid or the input data is invalid,  
        /// 404 NotFound if no permission is found with the provided ID,  
        /// 500 InternalServerError if an error occurs during the update process.
        /// </returns>
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

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "error while saving in DataBase");
            }
        }

        /// <summary>
        /// Deletes a specific permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission to delete.</param>
        /// <returns>
        /// 200 OK if the deletion is successful,  
        /// 400 BadRequest if the ID is invalid,  
        /// 500 InternalServerError if an error occurs during the deletion process.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            try
            {
            
                await permissionService.DeleteAsync(id);
                await permissionService.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500,"an error occurs while  deleting");

            }
        }
    }
}



