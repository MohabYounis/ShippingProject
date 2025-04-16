using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.ModelService;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly IServiceGeneric<Delivery> deliveryService;
        private readonly IServiceGeneric<Merchant> merchantService;
        private readonly IServiceGeneric<Employee> employeeService;

        public UserController(UserManager<ApplicationUser>usermanager ,IServiceGeneric<Delivery> deliveryService,IServiceGeneric<Merchant> merchantService,IServiceGeneric<Employee>employeeService)
        {
           
            this.usermanager = usermanager;
            this.deliveryService = deliveryService;
            this.merchantService = merchantService;
            this.employeeService = employeeService;
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetUser(string id, string Role)
        {
            try
            {
                var userManger = await usermanager.FindByIdAsync(id);
                if (userManger == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                Role = Role?.ToLower(); // نحول الـ Role إلى lowercase

                if (Role == "delivery")
                {
                    var user = await deliveryService.GetByUserIdAsync(id);
                    if (user == null)
                    {
                        return NotFound(new { success = false, message = "Delivery Not Found" });
                    }
                    return Ok(new { success = true, UserId = user.Id });
                }

                if (Role == "merchant")
                {
                    var user = await merchantService.GetByUserIdAsync(id);
                    if (user == null)
                    {
                        return NotFound(new { success = false, message = "Merchant Not Found" });
                    }
                    return Ok(new { success = true, UserId = user.Id });
                }

                if (Role != null && Role.StartsWith("employee"))
                {
                    var user = await employeeService.GetByUserIdAsync(id);
                    if (user == null)
                    {
                        return NotFound(new { success = false, message = "Employee Not Found" });
                    }
                    return Ok(new { success = true, UserId = user.Id });
                }

                return BadRequest(new { success = false, message = "Bad Request: Role is not recognized" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request", error = ex.Message });
            }
        }


    }
}
