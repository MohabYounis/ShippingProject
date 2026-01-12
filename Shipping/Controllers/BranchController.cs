using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.DTOs.Branch;
using Shipping.DTOs.MerchantDTOs;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Services;
using Shipping.Services.ModelService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IServiceGeneric<Branch> branchService;

        public BranchController(IServiceGeneric<Branch> branchService)
        {
            this.branchService = branchService;
        }

        [LogSensitiveActive]
        [HttpGet("{all:alpha}")]
        public async Task<ActionResult> GetWithPaginationAndSearch(string? searchTxt, string all = "all", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Branch> branches;
                if (all == "all") branches = await branchService.GetAllAsync();
                else if (all == "exist") branches = await branchService.GetAllExistAsync();
                else return BadRequest(GeneralResponse.Failure("Parameter Not Exist."));

                if (branches == null || !branches.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    if (!string.IsNullOrEmpty(searchTxt))
                    {
                        // Searching by name or phone
                        branches = branches
                            .Where(item =>
                                (item.Name?.Contains(searchTxt, StringComparison.OrdinalIgnoreCase) ?? false)
                            )
                            .ToList();

                        if (!branches.Any()) return NotFound(GeneralResponse.Failure("Not Found."));
                    }

                    var totalBranches = branches.Count();

                    // Pagination
                    var paginatedBranches = branches
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var branchDtos = paginatedBranches.Select(b => new BranchDTO
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Mobile = b.Mobile,
                        Location = b.Location,
                        CreatedDate = b.CreatedDate,
                        IsDeleted = b.IsDeleted,
                    }).ToList();

                    var result = new
                    {
                        TotalBranches = totalBranches,      // العدد الإجمالي للعناصر
                        Page = page,                        // الصفحة الحالية
                        PageSize = pageSize,                // عدد العناصر في الصفحة
                        Branches = branchDtos               // العناصر الحالية
                    };

                    return Ok(GeneralResponse.Success(result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
        

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var branch = await branchService.GetByIdAsync(id);
                if (branch == null) return NotFound(GeneralResponse.Failure("Not Found."));
                else
                {
                    var branchDto = new BranchDTO
                    {
                        Id = branch.Id,
                        Name = branch.Name,
                        Mobile = branch.Mobile,
                        Location = branch.Location,
                        CreatedDate = branch.CreatedDate,
                        IsDeleted = branch.IsDeleted
                    };

                    return Ok(GeneralResponse.Success(branchDto));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BranchCreateDTO branchDto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            try
            {
                if (!Regex.IsMatch(branchDto.Mobile, @"^(010|011|012|015)\d{8}$") &&
                    !Regex.IsMatch(branchDto.Mobile, @"^0\d{9}$"))
                        return BadRequest(GeneralResponse.Failure("Invalid mobile or landline number. Mobile must start with 010, 011, 012, or 015 and be 11 digits. Landline must start with 0 and be 10 digits."));


                var existingBranch = (await branchService.GetAllAsync())
                         .FirstOrDefault(b =>
                             b.Name == branchDto.Name &&
                             b.Mobile == branchDto.Mobile &&
                             b.Location == branchDto.Location &&
                             !b.IsDeleted);
                if (existingBranch != null)
                    return BadRequest(GeneralResponse.Failure("A branch with the same name, mobile, and location already exists."));


                var duplicateMobileBranch = (await branchService.GetAllAsync())
                                            .FirstOrDefault(b =>
                                                b.Name == branchDto.Name &&
                                                b.Mobile == branchDto.Mobile &&
                                                !b.IsDeleted);
                if (duplicateMobileBranch != null)
                    return BadRequest(GeneralResponse.Failure("A branch with the same name and mobile already exists."));


                var duplicateLocationBranch = (await branchService.GetAllAsync())
                                              .FirstOrDefault(b =>
                                                  b.Name == branchDto.Name &&
                                                  b.Location == branchDto.Location &&
                                                  !b.IsDeleted);
                if (duplicateLocationBranch != null)
                    return BadRequest(GeneralResponse.Failure("A branch with the same name and location already exists."));


                var duplicateMobileOnlyBranch = (await branchService.GetAllAsync())
                                     .FirstOrDefault(b =>
                                         b.Mobile == branchDto.Mobile &&
                                         !b.IsDeleted);
                if (duplicateMobileOnlyBranch != null)
                    return BadRequest(GeneralResponse.Failure("This mobile number is already registered with another branch."));


                var duplicateBranchName = await branchService.GetByNameAsync(branchDto.Name);
                if (duplicateBranchName != null)
                    return BadRequest(GeneralResponse.Failure("Branch name is already exist."));


                var branch = new Branch
                {
                    Name = branchDto.Name,
                    Mobile = branchDto.Mobile,
                    Location = branchDto.Location,
                };

                await branchService.AddAsync(branch);
                await branchService.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Branch Created Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] BranchEditDTO updatedBranchDto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage));
                return BadRequest(GeneralResponse.Failure(errors));
            }
            try
            {
                var existingBranch = await branchService.GetByIdAsync(id);
                if (existingBranch == null) return NotFound(GeneralResponse.Failure("Not Found."));

                existingBranch.Name = updatedBranchDto.Name;
                existingBranch.Mobile = updatedBranchDto.Mobile;
                existingBranch.Location = updatedBranchDto.Location;
                existingBranch.IsDeleted = updatedBranchDto.IsDeleted;

                await branchService.UpdateAsync(existingBranch);
                await branchService.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Branch updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await branchService.DeleteAsync(id);
                await branchService.SaveChangesAsync();
                return Ok(GeneralResponse.Success("Branch deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
    }
}