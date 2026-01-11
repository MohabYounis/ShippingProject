using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs;
using Shipping.DTOs.Employee;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.NewFolder1;
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
        public EmployeeController(IEmployeeService empService, IMapper mapper)
        {
            this.empService = empService;
        }
        [HttpGet("{all:alpha}")]
        public async Task<ActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<EmployeeGetDTO> employees;
                if (all == "all") employees = await empService.GetAllAsync();
                else if (all == "exist") employees = await empService.GetAllExistAsync();
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (employees == null || !employees.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        employees = employees
                            .Where(item =>
                                (item.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.Phone?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.BranchName?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (item.Email?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!employees.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    var totalEmployees = employees.Count();

                    var paginatedMerchants = employees
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                    var paginationDTO = new GenericPagination<EmployeeGetDTO>
                    {
                       TotalCount = totalEmployees,
                       PageNumber = page,
                       PageSize = pageSize,
                       Items = paginatedMerchants
                    };

                    return Ok(GeneralResponse.Success(paginationDTO));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                if (id <= 0) return BadRequest(GeneralResponse.Failure("Invalid ID!."));

                var employeeDTO = await empService.GetByIdAsync(id);
                return Ok(GeneralResponse.Success(employeeDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployee(CreateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            using var transaction = await empService.BeginTransactionAsync();
            try
            {
                await empService.AddAsync(employeeDto);
                await transaction.CommitAsync();

                return Ok(GeneralResponse.Success("Employee Created Successfully"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            using var transaction = await empService.BeginTransactionAsync();
            try
            {
                await empService.UpdateAsync(id, employeeDto);
                await transaction.CommitAsync();

                return Ok(GeneralResponse.Success("Employee updated successfully."));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            using var transaction = await empService.BeginTransactionAsync();
            try
            {
                await empService.DeleteAsync(id);
                await transaction.CommitAsync();

                return Ok(GeneralResponse.Success("Employee deleted successfully."));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
    }
}
