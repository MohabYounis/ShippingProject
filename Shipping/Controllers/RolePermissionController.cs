using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.RolePermission;
using Shipping.Enums;
using Shipping.Models;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        IRolePermissionService rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            this.rolePermissionService = rolePermissionService;
        }

        /// <summary>
        /// Retrieves all role permissions, optionally including deleted ones.
        /// </summary>
        /// <param name="includeDelted">A boolean flag indicating whether to include deleted role permissions (default is true).</param>
        /// <returns>
        /// 200 OK with a list of role permissions,  
        /// 404 Not Found if no role permissions are found.
        /// </returns>
        //get all
        [HttpGet("All")]
        public async Task<IActionResult> GetAllRolePermissions([FromQuery] bool includeDelted = true)
        {
            IEnumerable<RolePermission>? rolePermissions;

            if (includeDelted)
            {
                rolePermissions = await rolePermissionService.GetAllAsync();
            }
            else
            {
                rolePermissions = await rolePermissionService.GetAllExistAsync();
            }


            if (rolePermissions == null || !rolePermissions.Any())
            {
                return NotFound("there is no rolePermissions ");
            }
            //mapping to DTO
            var rolePermissionDtos = rolePermissions.Select(rp => new RolePermissionDTO
            {
                Role_Id = rp.Role_Id,
                Permission_Id = rp.Permission_Id,
                RoleName = rp.Role?.Name,
                PermissionName = rp.Permission?.Name,
                IsDeleted = rp.IsDeleted,
                CanEdit = rp.CanEdit,
                CanView = rp.CanView,
                CanAdd = rp.CanAdd,
                CanDelete = rp.CanDelete

            }).ToList();

            return Ok(rolePermissionDtos);
        }

        /// <summary>
        /// Retrieves a specific role permission based on the role ID and permission ID.
        /// </summary>
        /// <param name="role_id">The ID of the role.</param>
        /// <param name="permission_id">The ID of the permission.</param>
        /// <returns>
        /// 200 OK with the role permission details,  
        /// 404 Not Found if the role permission with the specified IDs does not exist.
        /// </returns>
        //get one row
        [HttpGet("{role_id}/{permission_id}")]
        public async Task<IActionResult> GetRolePermission(string role_id, int permission_id)
        {
            //getting from db
            var rolePermission = await rolePermissionService.GetRolePermissin(role_id, permission_id);
            if (rolePermission == null)
            {
                return NotFound($"from add action RolePermission with RoleId {role_id} and PermissionId {permission_id} not found.");
            }
            //mapping to DTO
            var rolePermissionDTO = new RolePermissionDTO
            {
                Role_Id = rolePermission.Role_Id,
                Permission_Id = rolePermission.Permission_Id,
                RoleName = rolePermission.Role?.Name,
                PermissionName = rolePermission.Permission?.Name,
                IsDeleted = rolePermission.IsDeleted,
                CanEdit = rolePermission.CanEdit,
                CanView = rolePermission.CanView,
                CanAdd = rolePermission.CanAdd,
                CanDelete = rolePermission.CanDelete
            };
            return Ok(rolePermissionDTO);
        }

        /// <summary>
        /// Adds a new role permission.
        /// </summary>
        /// <param name="role_id">The ID of the role to assign the permission to.</param>
        /// <param name="permission_id">The ID of the permission to be assigned.</param>
        /// <param name="rolePermissionDTO">The data of the role permission to be added.</param>
        /// <returns>
        /// 201 Created if the role permission is added successfully,  
        /// 400 BadRequest if model validation fails,  
        /// 404 NotFound if role or permission is not found,  
        /// 409 Conflict if the role permission already exists.
        /// </returns>
        // add role permission
        [HttpPost("{role_id}/{permission_id}")]
        public async Task<IActionResult> AddRolePermissin(string role_id, int permission_id, [FromBody] CreateRolePermission rolePermissionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //mapping
                var rolePermission = new RolePermission
                {
                    Role_Id = role_id,
                    Permission_Id = permission_id,
                    CanEdit = rolePermissionDTO.CanEdit,
                    CanView = rolePermissionDTO.CanView,
                    CanAdd = rolePermissionDTO.CanAdd,
                    CanDelete = rolePermissionDTO.CanDelete
                };
                //add
                var query = await rolePermissionService.AddRolePermission(rolePermission);
                switch (query)
                {
                    case AddResult.NotFound: return NotFound("Role or Permission not found");
                    case AddResult.AlreadyExists: return Conflict("RolePermission already exists");
                    case AddResult.AddedSuccessfully:
                        await rolePermissionService.SaveChangesAsync();
                        return CreatedAtAction("AddRolePermissin", new { role_id, permission_id }, "RolePermission added successfully");
                    default: return StatusCode(500, "Failed to add RolePermission");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing role permission by role ID and permission ID.
        /// </summary>
        /// <param name="role_id">The ID of the role.</param>
        /// <param name="permission_id">The ID of the permission.</param>
        /// <param name="rolePermissionDTO">The updated role permission data.</param>
        /// <returns>
        /// 200 OK if the update is successful,  
        /// 404 NotFound if the role permission is not found,  
        /// 409 Conflict if the role permission is already deleted,  
        /// 500 InternalServerError if the update fails.
        /// </returns>
        // update role permission
        [HttpPut("{role_id}/{permission_id}")]
        public async Task<IActionResult> UpdateRolePermission(string role_id, int permission_id, [FromBody] UpdateRolePermission rolePermissionDTO)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                //getting role permission
                var rolePermission = await rolePermissionService.GetRolePermissin(role_id, permission_id);
                if (rolePermission == null)
                {
                    return NotFound($"nilggf dddfd ffvvv RolePermission with RoleId {role_id} and PermissionId {rolePermissionDTO.Permission_Id} not found.");
                }


                //mapping
                rolePermission.CanEdit = rolePermissionDTO.CanEdit;
                rolePermission.CanView = rolePermissionDTO.CanView;
                rolePermission.CanAdd = rolePermissionDTO.CanAdd;
                rolePermission.CanDelete = rolePermissionDTO.CanDelete;
                //update
                var query = await rolePermissionService.UpdateRolePermissin(rolePermission);

                switch (query)
                {
                    case UpdateREsult.NotFound: return NotFound("RolePermission not found");

                    case UpdateREsult.UpdatedSuccessfully:
                        await rolePermissionService.SaveChangesAsync();

                        return Ok();

                    case UpdateREsult.AlreadyDeleted: return Conflict();

                    default: return StatusCode(500, "Failed to update RolePermission");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a role permission by role ID and permission ID.
        /// </summary>
        /// <param name="role_id">The ID of the role.</param>
        /// <param name="permission_id">The ID of the permission.</param>
        /// <returns>
        /// 200 OK if the role permission is deleted successfully,  
        /// 404 NotFound if the role permission is not found,  
        /// 409 Conflict if the role permission is already deleted,  
        /// 500 InternalServerError if the deletion fails.
        /// </returns>
        //delete role permission
        [HttpDelete("{role_id}/{permission_id}")]
        public async Task<IActionResult> DeleteRolePermission(string role_id, int permission_id)
        {
            try
            {
                var result = await rolePermissionService.DeleteRolePermissin(role_id, permission_id);

                switch (result)
                {
                    case DeleteResult.NotFound:
                        return NotFound("RolePermission not found");
                    case DeleteResult.DeletedSuccessfully:
                        await rolePermissionService.SaveChangesAsync();

                        return Ok("Deleted successfully");
                    case DeleteResult.AlreadyDeleted:
                        return Conflict("RolePermission already deleted");
                    default:
                        return StatusCode(500, "Failed to delete RolePermission");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
