using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.Role;
using Shipping.DTOs.RolePermission;
using Shipping.Models;
using Shipping.Services.IModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IApplicationRoleService _roleService;
        UserManager<ApplicationUser> userManger;
        RoleManager<ApplicationRole> roleManager;

        public RoleController(IApplicationRoleService roleService, UserManager<ApplicationUser> userManger, RoleManager<ApplicationRole> roleManager)
        {
            _roleService = roleService;
            this.userManger = userManger;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves all roles from the system.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of roles,  
        /// 404 Not Found if no roles are found.
        /// </returns>
        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            IEnumerable<ApplicationRole>? roles;
           
             roles = await _roleService.GetAllAsyncExist();
            //check null
            if (roles == null)
            {
                return NotFound("No roles found.");
            }
            //mapping
            var roleDTO = roles.Select(role => new AppRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                IsDeleted = role.IsDeleted,
                RolePermissions = role.RolePermissions
            .Select(rp => new RolePermissionDTO
            {
                Permission_Id = rp.Permission_Id,
                Role_Id = rp.Role_Id,
                CanView = rp.CanView,
                CanEdit = rp.CanEdit,
                CanDelete = rp.CanDelete,
                CanAdd = rp.CanAdd,
                IsDeleted = rp.IsDeleted

            }).ToList()
            }).ToList();

            return Ok(roleDTO);
        }

        /// <summary>
        /// Retrieves a specific role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>
        /// 200 OK with the role details,  
        /// 404 Not Found if the role with the specified ID does not exist.
        /// </returns>
        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
           
            var role = await _roleService.GetByIdAsync(id);
            
            if (role == null)
                return NotFound($"Role with ID {id} not found.");
            var roleDTO = new AppRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                IsDeleted = role.IsDeleted,
                RolePermissions = role.RolePermissions.Select(rp=>new RolePermissionDTO
                {
                    Permission_Id = rp.Permission_Id,
                    Role_Id = rp.Role_Id,
                    CanAdd = rp.CanAdd,
                    CanDelete = rp.CanDelete,
                    CanEdit = rp.CanEdit,
                    CanView = rp.CanView,
                }).ToList(),
            };

            return Ok(roleDTO);
        }

        /// <summary>
        /// Adds a new role to the system.
        /// </summary>
        /// <param name="role">The role data to be added.</param>
        /// <returns>
        /// 201 Created if the role is added successfully,  
        /// 400 BadRequest if the role already exists or model validation fails.
        /// </returns>
        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleDTO role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check if exist
                var existingRole = await _roleService.GetByNameAsync(role.Name);
                if (existingRole != null)
                {
                    return BadRequest("Role with name is already exists.");
                }

                // Mapping DTO → Entity
                var roleDB = new ApplicationRole
                {
                    Name = role.Name,
                };

                await roleManager.CreateAsync(roleDB);
                await _roleService.SaveDB();

                return Ok("Created!.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="role">The updated role data.</param>
        /// <returns>
        /// 204 NoContent if the update is successful,  
        /// 404 Not Found if the role with the specified ID does not exist,  
        /// 400 BadRequest if model validation fails.
        /// </returns>
        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleDTO role)
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

                // mapping 
                existingRole.Name = role.Name;

                _roleService.Update(existingRole);
                await _roleService.SaveDB();

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"error occured while updating");
            }
        }

        /// <summary>
        /// Deletes an existing role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>
        /// 204 NoContent if the role is deleted successfully,  
        /// 404 Not Found if the role with the specified ID does not exist,  
        /// 400 BadRequest if the role has already been deleted.
        /// </returns>
        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var existingRole = await _roleService.GetByIdAsync( id); 

                if (existingRole == null) return NotFound("no roles found"); 

                if (existingRole.IsDeleted) return BadRequest("already delted");
                 _roleService.Delete(existingRole);
                await _roleService.SaveDB();

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Role with ID  not found.");
            }
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="UserId">The ID of the user.</param>
        /// <param name="RoleName">The name of the role to assign to the user.</param>
        /// <returns>
        /// 200 OK if the role is assigned successfully,  
        /// 400 BadRequest if the user or role does not exist or if an error occurs.
        /// </returns>
        // POST: api/Role/AssignRole
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(string UserId , string RoleName)
        {
            try
            {
                //check user
                var user = await userManger.FindByIdAsync(UserId);
                if (user == null)
                {
                    return BadRequest("User not found");
                }

                //check role
                var existingRole = await _roleService.GetByNameAsync(RoleName);
                if (existingRole == null)
                {
                    return BadRequest("Role with this ID does not exist.");
                }

                //assign role
                var result = await userManger.AddToRoleAsync(user, existingRole.Name);
                if (!result.Succeeded)
                {
                    return BadRequest($"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return Ok($"Role '{existingRole.Name}' assigned to user '{user.UserName}' successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "gfdsssss");
            }
        }
    }
}
