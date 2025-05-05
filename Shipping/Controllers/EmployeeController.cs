using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.Employee;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.pagination;
using Shipping.Models;
using Shipping.Services;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using System.Net;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IEmployeeService empService;
        IMapper mapper;
        public EmployeeController(IEmployeeService empService, IMapper mapper)
        {
            this.empService = empService;
            this.mapper = mapper;
        }


        /// <summary>
        /// Retrieves a paginated list of employees.
        /// </summary>
        /// <param name="includeDeleted">
        /// Indicates whether to include soft-deleted employees in the result. Default is <c>true</c>.
        /// </param>
        /// <param name="pageIndex">
        /// The page index for pagination. Default is <c>1</c>.
        /// </param>
        /// <param name="pageSize">
        /// The number of employees per page. Default is <c>10</c>.
        /// </param>
        /// <returns>
        /// A paginated list of employees. By default, returns the first 10 existing employees (including deleted if any).
        /// </returns>
        /// <response code="200">Employees retrieved successfully.</response>
        /// <response code="404">there is no employees .</response>
        [HttpGet("{all:alpha}")]
        public async Task<ActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Employee> employees;
                if (all == "all") employees = await empService.GetAllAsync();
                else if (all == "exist") employees = await empService.GetAllExistAsync();
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (employees == null || !employees.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching by name or phone
                        employees = employees
                            .Where(item =>
                                (item.ApplicationUser.UserName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.ApplicationUser.PhoneNumber?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!employees.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    var totalEmployees = employees.Count();

                    // Pagination
                    var paginatedMerchants = employees
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                        .ToList();

                    List<MerchantGetDTO> merchantDTO = mapper.Map<List<MerchantGetDTO>>(paginatedMerchants);


                    var result = new
                    {
                        TotalMerchants = totalEmployees,       // العدد الإجمالي للعناصر
                        Page = page,                          // الصفحة الحالية
                        PageSize = pageSize,                  // عدد العناصر في الصفحة
                        Merchants = merchantDTO               // العناصر الحالية
                    };

                    return Ok(GeneralResponse.Success(result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        /// <summary>
        /// Retrieves a specific employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee details if found.</returns>
        /// <response code="200">Employee retrieved successfully.</response>
        /// <response code="400">Invalid ID.</response>
        /// <response code="404">Employee not found with the specified ID.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var employeeDto = await empService.GetByIdAsync(id);
            if (employeeDto == null)
                return NotFound($"ID {id} not found");

            return Ok(employeeDto);
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employeeDto">The employee data to be created.</param>
        /// <returns>The created employee with generated ID and details.</returns>
        /// <response code="201">Employee created successfully.</response>
        /// <response code="400">Invalid input data or model validation failed.</response>
        /// <response code="500">An employee with similar unique data already exists (e.g., email).</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateEmployeeDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {
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

        /// <summary>
        /// Updates an existing employee's information.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employeeDto">The updated employee data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Employee updated successfully.</response>
        /// <response code="400">Invalid input data or model validation failed.</response>
        /// <response code="404">Employee not found with the specified ID.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes a company by ID.
        /// </summary>
        /// <param name="id">The ID of the company to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Company deleted successfully.</response>
        /// <response code="500">Company not found.</response>
        /// <response code="400">Invalid ID</response>
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

        /// <summary>
        /// Retrieves a list of employees by their role.
        /// </summary>
        /// <param name="roleName">The name of the role to filter employees by.</param>
        /// <returns>A list of employees with the specified role.</returns>
        /// <response code="200">Employees retrieved successfully.</response>
        /// <response code="500">No employees found for the specified role.</response>
        [HttpGet("GetEmployeesByRole")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Searches for employees by name.
        /// </summary>
        /// <param name="term">The search term to match against employee names.</param>
        /// <param name="includeDeleted">
        /// Indicates whether to include soft-deleted employees in the results. Default is <c>true</c>.
        /// </param>
        /// <returns>A list of employees whose names match the search term.</returns>
        /// <response code="200">Employees matching the search term retrieved successfully.</response>
        /// <response code="500">Search term is invalid or missing.</response>
        [HttpGet("SearchByName")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
