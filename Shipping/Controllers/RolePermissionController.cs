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


        //get all

        [HttpGet("All")]

        public async Task<IActionResult> GetAllRolePermissions()
        {
            //getting from db
            var rolePermissions = await rolePermissionService.GetAllAsync();
            if (rolePermissions == null || !rolePermissions.Any())
            {
                return NotFound("there is no rolePermissions ");
            }

            //mapping to DTO
            var rolePermissionDtos = rolePermissions.Select(rp => new RolePermissionDTO
            {
                Role_Id = rp.Role_Id,
                Permission_Id = rp.Permission_Id,
                //RoleName = rp.Role.Name,
                //PermissionName = rp.Permission.Name,
                IsDeleted = rp.IsDeleted,
                CanEdit= rp.CanEdit,
                CanView = rp.CanView,
                CanAdd = rp.CanAdd,
                CanDelete = rp.CanDelete

            }).ToList();




            return Ok(rolePermissionDtos);
        }

        //get all

        [HttpGet("Exist")]

        public async Task<IActionResult> GetAllRolePermissionsExist()
        {
            //getting from db
            var rolePermissions = await rolePermissionService.GetAllExistAsync();
            if (rolePermissions == null || !rolePermissions.Any())
            {
                return NotFound("there is no rolePermissions ");
            }

            //mapping to DTO
            var rolePermissionDtos = rolePermissions.Select(rp => new RolePermissionDTO
            {
                Role_Id = rp.Role_Id,
                Permission_Id = rp.Permission_Id,
                //RoleName = rp.Role.Name,
                //PermissionName = rp.Permission.Name,
                IsDeleted = rp.IsDeleted,
                CanEdit = rp.CanEdit,
                CanView = rp.CanView,
                CanAdd = rp.CanAdd,
                CanDelete = rp.CanDelete

            }).ToList();




            return Ok(rolePermissionDtos);
        }


        //get one row
        [HttpGet("{role_id}/{permission_id}")]
        public async Task<IActionResult> GetRolePermission(string role_id, int permission_id)
        {
            //getting from db
            var rolePermission = await rolePermissionService.GetRolePermissin(role_id, permission_id);
            if (rolePermission == null)
            {
                return NotFound($"RolePermission with RoleId {role_id} and PermissionId {permission_id} not found.");
            }
            //mapping to DTO
            var rolePermissionDTO = new RolePermissionDTO
            {
                Role_Id = rolePermission.Role_Id,
                Permission_Id = rolePermission.Permission_Id,
                //RoleName = rolePermission.Role.Name,
                //PermissionName = rolePermission.Permission.Name,
                IsDeleted = rolePermission.IsDeleted,
                CanEdit = rolePermission.CanEdit,
                CanView = rolePermission.CanView,
                CanAdd = rolePermission.CanAdd,
                CanDelete = rolePermission.CanDelete
            };
            return Ok(rolePermissionDTO);
        }


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

            // update role permission
            [HttpPut("{role_id}/{permission_id}")]

        public async Task<IActionResult> UpdateRolePermission(string role_id, int permission_id, [FromBody] UpdateRolePermission rolePermissionDTO)
     
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try { 
            //getting role permission
            var rolePermission = await rolePermissionService.GetRolePermissin(role_id, permission_id);
            if (rolePermission == null)
            {
                return NotFound($"RolePermission with RoleId {role_id} and PermissionId {permission_id} not found.");
            }
            
           
                //mapping
                rolePermission.CanEdit = rolePermissionDTO.CanEdit;
                rolePermission.CanView = rolePermissionDTO.CanView;
                rolePermission.CanAdd = rolePermissionDTO.CanAdd;
                rolePermission.CanDelete = rolePermissionDTO.CanDelete;
                //update
              var query =  await rolePermissionService.UpdateRolePermissin(rolePermission);

                switch (query)
                {
                    case UpdateREsult.NotFound:  return NotFound("RolePermission not found");

                    case UpdateREsult.UpdatedSuccessfully:
                      await  rolePermissionService.SaveChangesAsync();

                        return Ok("updated successfully");

                    case UpdateREsult.AlreadyDeleted: return Conflict("RolePermission already deleted");

                    default:return StatusCode(500, "Failed to update RolePermission");

                }

            
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }





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
                      await  rolePermissionService.SaveChangesAsync();

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
