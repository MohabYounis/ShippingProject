using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
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

        UserManager<ApplicationUser> userManager;
        IEmployeeService empService;

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



        [HttpGet("{id}")]
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
        public async Task<IActionResult> AddEmployee(EmployeeDTO employeeDto)
        {

            if (employeeDto == null)
            {
                return BadRequest("Employee is null");
            }

            // getting app user from employeeDto
            ApplicationUser appUser = null;
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
            // Get branchId from database using BranchName

            var branch = branchService.GetByIdAsync(employeeDto.branchId);
            if (branch == null) { return BadRequest("branch not found"); }
            //   mapping manually employeeDto to employee
            Employee emp = new Employee()
            {
                Branch_Id = branch.Id,
                AppUser_Id = appUser.Id,
                ApplicationUser = appUser,
            };

            await employeeService.AddAsync(emp);

            await employeeService.SaveChangesAsync();


            return Ok("employee added successfully!");
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
            {
                return NotFound($"Employee with id {id} not found");
            }

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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");
            var employee = await empService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }
            await empService.DeleteAsync(id);
            await empService.SaveChangesAsync();
            return Ok("Employee deleted successfully!");
        }









    }
}
