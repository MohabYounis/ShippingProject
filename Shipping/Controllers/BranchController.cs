using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.DTOs.Branch;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Services;
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


        // Get All Branches
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await branchService.GetAllAsync();
            var branchDtos = branches.Select(b => new BranchDTO
            {
                Id = b.Id,
                Name = b.Name,
                Mobile = b.Mobile,
                Location = b.Location,
                CreatedDate = b.CreatedDate,
                IsDeleted = b.IsDeleted,
            }).ToList();

            return Ok(branchDtos);
        }

        // Get Branch by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResponse>> GetById(int id)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                var branch = await branchService.GetByIdAsync(id);

                var branchDto = new BranchDTO
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    Mobile = branch.Mobile,
                    Location = branch.Location,
                    CreatedDate = branch.CreatedDate,
                    IsDeleted = branch.IsDeleted
                };

                response.IsSuccess = true;
                response.Data = branchDto;
            }
            catch (KeyNotFoundException)
            {
                response.IsSuccess = false;
                response.Data = $"Branch with ID {id} was not found.";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);

            //var branch = await branchService.GetByIdAsync(id);
            //if (branch == null) return NotFound();

            //var branchDto = new BranchDTO
            //{
            //    Id = branch.Id,
            //    Name = branch.Name,
            //    Mobile = branch.Mobile,
            //    Location = branch.Location,
            //    CreatedDate = branch.CreatedDate,
            //    IsDeleted = branch.IsDeleted
            //};

            //return Ok(branchDto);
        }


        // Add New Branch
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchDTO branchDto)
        {
            if (branchDto == null) return BadRequest("The data is incorrect");


            if (!Regex.IsMatch(branchDto.Mobile, @"^(010|011|012|015)\d{8}$") &&
                 !Regex.IsMatch(branchDto.Mobile, @"^0\d{9}$"))
            {
                return BadRequest("Invalid mobile or landline number. Mobile must start with 010, 011, 012, or 015 and be 11 digits. Landline must start with 0 and be 10 digits.");
            }

            var existingBranch = (await branchService.GetAllAsync())
                     .FirstOrDefault(b =>
                         b.Name == branchDto.Name &&
                         b.Mobile == branchDto.Mobile &&
                         b.Location == branchDto.Location &&
                         !b.IsDeleted);

            if (existingBranch != null)
            {
                return Conflict("A branch with the same name, mobile, and location already exists.");
            }

            var duplicateMobileBranch = (await branchService.GetAllAsync())
                                        .FirstOrDefault(b =>
                                            b.Name == branchDto.Name &&
                                            b.Mobile == branchDto.Mobile &&
                                            !b.IsDeleted);

            if (duplicateMobileBranch != null)
            {
                return Conflict("A branch with the same name and mobile already exists.");
            }

            var duplicateLocationBranch = (await branchService.GetAllAsync())
                                          .FirstOrDefault(b =>
                                              b.Name == branchDto.Name &&
                                              b.Location == branchDto.Location &&
                                              !b.IsDeleted);

            if (duplicateLocationBranch != null)
            {
                return Conflict("A branch with the same name and location already exists.");
            }

            var duplicateMobileOnlyBranch = (await branchService.GetAllAsync())
                                 .FirstOrDefault(b =>
                                     b.Mobile == branchDto.Mobile &&
                                     !b.IsDeleted);

            if (duplicateMobileOnlyBranch != null)
            {
                return Conflict("This mobile number is already registered with another branch.");
            }

            var branch = new Branch
            {
                Name = branchDto.Name,
                Mobile = branchDto.Mobile,
                Location = branchDto.Location,
            };

            await branchService.AddAsync(branch);
            await branchService.SaveChangesAsync();
            
            var createdBranch = await branchService.GetByIdAsync(branch.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdBranch.Id }, createdBranch);
        }


        // Update Branch by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditBranchDTO updatedBranchDto)
        {
            GeneralResponse response = new GeneralResponse();

            if (updatedBranchDto == null)
            {
                response.IsSuccess = false;
                response.Data = "The data is incorrect.";
                return BadRequest(response);
            }

            try
            {
                var existingBranch = await branchService.GetByIdAsync(id);
                if (existingBranch == null)
                {
                    response.IsSuccess = false;
                    response.Data = $"Branch with ID {id} was not found.";
                    return NotFound(response);
                }

                // Update properties
                existingBranch.Name = updatedBranchDto.Name;
                existingBranch.Mobile = updatedBranchDto.Mobile;
                existingBranch.Location = updatedBranchDto.Location;
                existingBranch.CreatedDate = updatedBranchDto.CreatedDate;

                await branchService.UpdateAsync(id);
                await branchService.SaveChangesAsync();

                // Prepare response with updated data
                var updatedBranchData = new EditBranchDTO
                {
                    Id = existingBranch.Id,
                    Name = existingBranch.Name,
                    Mobile = existingBranch.Mobile,
                    Location = existingBranch.Location,
                    CreatedDate = existingBranch.CreatedDate,
                };

                response.IsSuccess = true;
                response.Data = updatedBranchData;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
            return Ok(response);
        }


        // Soft Delete Branch
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await branchService.GetByIdAsync(id);
            if (branch == null) return NotFound();

            branch.IsDeleted = true;
            branchService.UpdateAsync(id);
            //_branchRepository.SaveDB();

            await branchService.DeleteAsync(id);
            await branchService.SaveChangesAsync();
            return Ok(new { message = "Branch Successfully Deleted " });
        }
    }
}