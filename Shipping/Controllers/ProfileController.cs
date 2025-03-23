using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.Branch;
using Shipping.DTOs;
using Shipping.Services.ModelService;
using Shipping.Models;
using Shipping.Services;
using Microsoft.AspNetCore.Identity;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        // Get User by ID
        [HttpGet("id")]
        public async Task<ActionResult<GeneralResponse>> GetById(string id)
        {
           var user=  await  userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            var role = await userManager.GetRolesAsync(user);
            ProfileDto profileDto = new ProfileDto()
            {
               UserName=user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = role.ToList(),
                Address = user.Address
            };
            return Ok(profileDto);
        }
    }
}
