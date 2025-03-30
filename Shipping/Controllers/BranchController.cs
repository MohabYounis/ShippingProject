using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.DTOs.Branch;
using Shipping.DTOs.MerchantDTOs;
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
        private readonly GeneralResponse response;

        public BranchController(IServiceGeneric<Branch> branchService, GeneralResponse response)
        {
            this.branchService = branchService;
            this.response = response;
        }


        // Get All Branches
        [HttpGet("All")]
        public async Task<ActionResult<GeneralResponse>> GetAll()
        {
            try
            {
                var branches = await branchService.GetAllAsync();
                if (branches == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                }
                else
                {
                    var branchDtos = branches.Select(b => new BranchDTO
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Mobile = b.Mobile,
                        Location = b.Location,
                        CreatedDate = b.CreatedDate,
                        IsDeleted = b.IsDeleted,
                    }).ToList();

                    response.IsSuccess = true;
                    response.Data = branchDtos;
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }

        // Get all branches that are not deleted
        [HttpGet("AllExist")]
        public async Task<ActionResult<GeneralResponse>> GetAllExist()
        {
            try
            {
                var branches = await branchService.GetAllExistAsync();
                if (branches == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                }
                else
                {
                    var branchDtos = branches.Select(b => new BranchDTO
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Mobile = b.Mobile,
                        Location = b.Location,
                        CreatedDate = b.CreatedDate,
                        IsDeleted = b.IsDeleted,
                    }).ToList();

                    response.IsSuccess = true;
                    response.Data = branchDtos;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }



        // Get Branch by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> GetById(int id)
        {
            try
            {
                var branch = await branchService.GetByIdAsync(id);
                if (branch == null)
                {
                    response.IsSuccess = false;
                    response.Data = "No Found";
                }
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

                    response.IsSuccess = true;
                    response.Data = branchDto;
                }
            }
            catch (KeyNotFoundException ex)
            {
                response.IsSuccess = false;
                response.Data = $"Branch with ID {id} was not found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }


        // Add New Branch
        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> Create([FromBody] CreateAndEditBranchDTO branchDto)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
            }
            try
            {
                if (!Regex.IsMatch(branchDto.Mobile, @"^(010|011|012|015)\d{8}$") &&
                 !Regex.IsMatch(branchDto.Mobile, @"^0\d{9}$"))
                {
                    response.IsSuccess = false;
                    response.Data = "Invalid mobile or landline number. Mobile must start with 010, 011, 012, or 015 and be 11 digits. Landline must start with 0 and be 10 digits.";
                    return BadRequest(response);
                }

                var existingBranch = (await branchService.GetAllAsync())
                         .FirstOrDefault(b =>
                             b.Name == branchDto.Name &&
                             b.Mobile == branchDto.Mobile &&
                             b.Location == branchDto.Location &&
                             !b.IsDeleted);

                if (existingBranch != null)
                {
                    response.IsSuccess = false;
                    response.Data = "A branch with the same name, mobile, and location already exists.";
                    return BadRequest(response);
                }

                var duplicateMobileBranch = (await branchService.GetAllAsync())
                                            .FirstOrDefault(b =>
                                                b.Name == branchDto.Name &&
                                                b.Mobile == branchDto.Mobile &&
                                                !b.IsDeleted);

                if (duplicateMobileBranch != null)
                {
                    response.IsSuccess = false;
                    response.Data = "A branch with the same name and mobile already exists.";
                    return BadRequest(response);
                }

                var duplicateLocationBranch = (await branchService.GetAllAsync())
                                              .FirstOrDefault(b =>
                                                  b.Name == branchDto.Name &&
                                                  b.Location == branchDto.Location &&
                                                  !b.IsDeleted);

                if (duplicateLocationBranch != null)
                {
                    response.IsSuccess = false;
                    response.Data = "A branch with the same name and location already exists.";
                    return BadRequest(response);
                }

                var duplicateMobileOnlyBranch = (await branchService.GetAllAsync())
                                     .FirstOrDefault(b =>
                                         b.Mobile == branchDto.Mobile &&
                                         !b.IsDeleted);

                if (duplicateMobileOnlyBranch != null)
                {
                    response.IsSuccess = false;
                    response.Data = "This mobile number is already registered with another branch.";
                    return BadRequest(response);
                }

                var duplicateBranchName = await branchService.GetByNameAsync(branchDto.Name);

                if (duplicateBranchName != null)
                {
                    response.IsSuccess = false;
                    response.Data = "Branch name is already exist.";
                    return BadRequest(response);
                }

                var branch = new Branch
                {
                    Name = branchDto.Name,
                    Mobile = branchDto.Mobile,
                    Location = branchDto.Location,
                };

                await branchService.AddAsync(branch);
                await branchService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Branch Created Successfully";
                return CreatedAtAction("Create", response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, ex.Message);
            }
        }


        // Update Branch by ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> Update(int id, [FromBody] CreateAndEditBranchDTO updatedBranchDto)
        {
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Data = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
            }
            try
            {
                var existingBranch = await branchService.GetByIdAsync(id);
                if (existingBranch == null)
                {
                    response.IsSuccess = false;
                    response.Data = $"Branch with ID {id} was not found.";
                    return response;
                }

                // Update properties
                existingBranch.Name = updatedBranchDto.Name;
                existingBranch.Mobile = updatedBranchDto.Mobile;
                existingBranch.Location = updatedBranchDto.Location;

                await branchService.UpdateAsync(existingBranch);
                await branchService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Branch updated successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }

            return response;
        }


        // Soft Delete Branch
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GeneralResponse>> Delete(int id)
        {
            try
            {
                await branchService.DeleteAsync(id);
                await branchService.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = "Branch deleted successfully.";
            }
            catch (KeyNotFoundException ex) // لو انا دخلت id مش موجود
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }
            catch (InvalidOperationException ex) // id موجود بس انا كنت عامله soft delete
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }
            catch (Exception ex) // بيهندل اي exception جاي من ال server
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
            }
            return response;
        }
    }
}