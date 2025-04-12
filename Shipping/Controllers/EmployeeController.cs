using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.Employee;
using Shipping.DTOs.pagination;
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
        IServiceGeneric<Branch> branchService;

        UserManager<ApplicationUser> userManager;
        IEmployeeService empService;
        //
        IApplicationRoleService roleService;


        //
        public EmployeeController( IServiceGeneric<Employee> employeeService, IEmployeeService empService, UserManager<ApplicationUser> userManager, IServiceGeneric<Branch> branchService, IApplicationRoleService roleService)
        {
            this.userManager = userManager;
            this.branchService = branchService;
            this.empService = empService;
            this.roleService = roleService;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] bool includeDelted = true, int pageIndex = 1, int pageSize = 10)
        {
            GenericPagination<EmployeeDTO>? employeeDtos = null;

            if (!includeDelted) { employeeDtos = await empService.GetAllExistAsync(pageIndex,pageSize); }


            else employeeDtos = await empService.GetAllAsync(pageIndex, pageSize);

            if (employeeDtos.Items == null || !employeeDtos.Items.Any())
            {
                return NotFound("there is no employees ");
            }
            

            return Ok(employeeDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var employeeDto = await empService.GetByIdAsync(id);
            if (employeeDto == null)
                return NotFound($"ID {id} not found");

            return Ok(employeeDto);
        }



        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {
            // 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await empService.BeginTransactionAsync();
            try
            {
                //  add the employee
                var createdEmployee = await empService.AddAsync(employeeDto);
                // Commit  transaction
                await transaction.CommitAsync();

                return Ok(createdEmployee);
            }
            catch (Exception ex)
            {
               

                //  detailed error 
                var errorResponse = new
                {
                    Message = "An error occurred while adding the employee.",
                    Error = ex.Message,
                    InnerException = ex.InnerException?.Message,
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDTO employeeDto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Transaction
            using var transaction = await empService.BeginTransactionAsync();
            try
            {
                var updatedEmployee = await empService.UpdateAsync(id, employeeDto);
                if (updatedEmployee == null)
                    return NotFound($"Employee with ID {id} not found");

                await transaction.CommitAsync();
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        //[HttpDelete("{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            using var transaction = await empService.BeginTransactionAsync();
            try
            {
                var result = await empService.DeleteAsync(id);
            
                await transaction.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("GetEmployeesByRole")]
        public async Task<IActionResult> GetEmployeesByRole([FromQuery] string roleName)
        {
            try
            {
                var employeeDtos = await empService.GetEmployeesByRole(roleName);
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //search by name

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName([FromQuery] string term, [FromQuery] bool includeDeleted = true)
        {
            try
            {
                var employeeDtos = await empService.GetEmployeesBySearch(term, includeDeleted);
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
