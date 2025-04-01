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

        UserManager<ApplicationUser> userManager;
        IEmployeeService empService;
        //
        IApplicationRoleService roleService;

        public EmployeeController(IServiceGeneric<Employee> employeeService, IEmployeeService empService, UserManager<ApplicationUser> userManager, IServiceGeneric<Branch> branchService, IApplicationRoleService roleService)
        {
            this.employeeService = employeeService;
            this.userManager = userManager;
            this.branchService = branchService;
            this.empService = empService;
            this.roleService = roleService;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] bool includeDelted=true)
        {
            IEnumerable<Employee>? employees = null;

            if (!includeDelted) {  employees = await empService.GetAllExistAsync(); }


            employees = await empService.GetAllAsync();
            if (employees == null || !employees.Any())
            {
                return NotFound("there is no employees ");
            }

            List<EmployeeDTO> employeeDtos = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.ApplicationUser?.UserName,
                Address = e.ApplicationUser?.Address,
                userId = e.ApplicationUser?.Id,
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
                userId = employee.ApplicationUser?.Id,

                IsDeleted = employee.IsDeleted

            };

            return Ok(employeeDto);


        }



        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //
            var validRoles = new List<string> { "Employee", "BranchManager", "Sales" };


            if (!validRoles.Contains(employeeDto.Role.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid role: '{employeeDto.Role}'. Allowed roles: {string.Join(", ", validRoles)}");
            }

            // Get branch

            var branch = await branchService.GetByIdAsync(employeeDto.branchId);
            if (branch == null)
            {
                return BadRequest("branch not found");
            }
            // getting app user from employeeDto
            ApplicationUser appUser = null;

            //transaction
            using var transaction = await empService.BeginTransactionAsync(); // 🟢 فتح Transaction
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

  
                //check role
                var role= await roleService.GetByNameAsync(employeeDto.Role.Trim());
                if (role == null) return BadRequest($"Role {employeeDto.Role} not found");
                // assign role 
                await userManager.AddToRoleAsync(appUser, role.Name);


                //   mapping manually employeeDto to employee
                Employee emp = new Employee()
            {
                Branch_Id = branch.Id,
                AppUser_Id = appUser.Id,
                ApplicationUser = appUser,
            };

            await employeeService.AddAsync(emp);

            await employeeService.SaveChangesAsync();

                await transaction.CommitAsync();

            return Ok("employee added successfully!");
            
            }


              catch (Exception ex)
              {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");


              }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeDTO employeeDto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //transaction
            using var transaction = await empService.BeginTransactionAsync(); // 🟢 فتح Transaction
            try
            {
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
                await employeeService.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok("Employee updated successfully!");
            }

            catch (Exception ex) {
                await transaction.RollbackAsync(); 
                return StatusCode(500, $"An error occurred: {ex.Message}");


            }
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
            //transaction
            using var transaction = await empService.BeginTransactionAsync(); 
            try
            {
                // delete app user
                if (employee.ApplicationUser != null)
                {
                    var result = await userManager.DeleteAsync(employee.ApplicationUser);
                    if (!result.Succeeded)
                    {
                        return BadRequest("Failed to delete the user.");
                    }
                }
                //delete employee
                await employeeService.DeleteAsync(id);
                await employeeService.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok("Employee deleted successfully!");


            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");


            }
        }



        //by role name
        [HttpGet("GetEmployeesByRole")]
        public async Task<IActionResult> GetEmployeesByRole(string roleName)
        {
            try
            {
                var employees = await empService.GetEmployeesByRole(roleName);
                if (employees == null || !employees.Any()) return NotFound($"ther is no  employees have {roleName} role");
                //mapping
                var employeesDto = employees.Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    IsDeleted = e.IsDeleted,

                    userId = e.AppUser_Id,
                    Name = e.ApplicationUser.UserName,
                    Phone = e.ApplicationUser?.PhoneNumber,
                    Address = e.ApplicationUser?.Address,

                    branchId = e.Branch_Id

                }).ToList();



                return Ok(employeesDto);
            }

            catch (Exception ex) { 
            
            return StatusCode(500, ex.Message);
            }
        }

    }
}