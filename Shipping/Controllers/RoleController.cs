using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.Role;
using Shipping.DTOs.RolePermission;
using Shipping.Models;
using Shipping.Services.IModelService;
using System.Diagnostics.Eventing.Reader;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IApplicationRoleService _roleService;
        UserManager<ApplicationUser> userManger;

        public RoleController(IApplicationRoleService roleService, UserManager<ApplicationUser> roleManger)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            this.userManger = roleManger;
        }


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
                var existingRole = await _roleService.GetByIdAsync(role.Id);
                if (existingRole != null)
                {
                    return BadRequest("Role with this ID already exists.");
                }

                // Mapping DTO → Entity
                var roleDB = new ApplicationRole
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsDeleted = role.IsDeleted,
                    NormalizedName = role.NormalizedName
                };

                await _roleService.AddAsync(roleDB);
                await _roleService.SaveDB();

                return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, roleDB);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
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


        //assign role to user
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
