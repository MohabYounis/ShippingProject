using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.Employee;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using System.Net;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IServiceGeneric<Employee> employeeService;
        IServiceGeneric<Branch> branchService;
        IEmployeeService empService;
        UserManager<ApplicationUser> userManager;

        public EmployeeController(IServiceGeneric<Employee> employeeService, IEmployeeService empService, UserManager<ApplicationUser> userManager, IServiceGeneric<Branch> branchService)
        {
            this.employeeService = employeeService;
            this.userManager = userManager;
            this.branchService = branchService;
            this.empService = empService;
        }




        [HttpGet("All")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await empService.GetAllAsync();
            if (employees == null || !employees.Any())
            {
                return NotFound("there is no employees ");
            }

            List<EmployeeDTO> employeeDtos = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.ApplicationUser?.UserName,
                Address = e.ApplicationUser?.Address,
                branchId = e.Branch.Id,
                IsDeleted = e.IsDeleted
            }).ToList();

            return Ok(employeeDtos);
        }


        [HttpGet("exist")]
        public async Task<IActionResult> GetAllExistEmployees()
        {
            var employeesExist = await empService.GetAllExistAsync();
            if (employeesExist == null || !employeesExist.Any())
            {
                return NotFound("there is no employees ");
            }

            List<EmployeeDTO> employeeDtos = employeesExist.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.ApplicationUser?.UserName,
                Address = e.ApplicationUser?.Address,
                branchId = e.Branch.Id,
                IsDeleted = e.IsDeleted
            }).ToList();

            return Ok(employeeDtos);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");
            //getting emp drom db
            var employee = await empService.GetByIdAsync(id);
            if (employee == null) return NotFound($"  id {id}  not found");
            var employeeDto = new EmployeeDTO
            {

                Id = employee.Id,
                Name = employee.ApplicationUser?.UserName,
                Address = employee.ApplicationUser?.Address,
                branchId = employee.Branch.Id,
                IsDeleted = employee.IsDeleted

            };

            return Ok(employeeDto);
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                ));
            }
            try
            {

                appUser = new ApplicationUser()
                {
                    UserName = employeeDto.Name,
                    Email = employeeDto.Email,
                    PhoneNumber = employeeDto.Phone,
                    Address = employeeDto.Address
                };
                // creating user in database
                var result = await userManager.CreateAsync(appUser, employeeDto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("app" + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

            }
            catch (Exception ex)
            {
                return BadRequest("app" + ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeGetAndEditDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                ));
            }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDTO employeeDto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            if (employeeDto == null) return BadRequest("Employee is null");
            //getting employee from db
            var employee = await empService.GetByIdAsync(id);
            if (employee == null)
                return NotFound($"Employee with id {id} not found");

            // updating app user
            ApplicationUser appUser = employee.ApplicationUser;
            appUser.UserName = employeeDto.Name;
            appUser.Email = employeeDto.Email;
            appUser.PhoneNumber = employeeDto.Phone;
            appUser.Address = employeeDto.Address;
            var result = await userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            //mapping app user to employee
            employee.ApplicationUser = appUser;

            //mapping branch to employee
            var branch = (await branchService.GetByIdAsync(employeeDto.branchId));
            if (branch == null) return BadRequest("Branch not found");

            employee.Branch_Id = branch.Id;
            await empService.SaveChangesAsync();

            return Ok("Employee updated successfully!");
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid ID");
                var employee = await empService.GetByIdAsync(id);
                
                await empService.DeleteAsync(id);
                await empService.SaveChangesAsync();
                return Ok("Employee deleted successfully!");
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "The requested resource was not found." });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { Message = "The operation is invalid due to a logical constraint." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }









    }
}
