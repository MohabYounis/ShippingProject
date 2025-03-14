﻿using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs.Branch;
using Shipping.Models;
using Shipping.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {

        private readonly IRepositoryGeneric<Branch> _branchRepository;

        public BranchController(IRepositoryGeneric<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        // Get All Branches
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _branchRepository.GetAllAsync();
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
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null) return NotFound();

            var branchDto = new BranchDTO
            {
                Id = branch.Id,
                Name = branch.Name,
                Mobile = branch.Mobile,
                Location = branch.Location,
                CreatedDate = branch.CreatedDate,
                IsDeleted = branch.IsDeleted
            };

            return Ok(branchDto);
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

            var existingBranch = (await _branchRepository.GetAllAsync())
                     .FirstOrDefault(b =>
                         b.Name == branchDto.Name &&
                         b.Mobile == branchDto.Mobile &&
                         b.Location == branchDto.Location &&
                         !b.IsDeleted);

            if (existingBranch != null)
            {
                return Conflict("A branch with the same name, mobile, and location already exists.");
            }

            var duplicateMobileBranch = (await _branchRepository.GetAllAsync())
                                        .FirstOrDefault(b =>
                                            b.Name == branchDto.Name &&
                                            b.Mobile == branchDto.Mobile &&
                                            !b.IsDeleted);

            if (duplicateMobileBranch != null)
            {
                return Conflict("A branch with the same name and mobile already exists.");
            }

            var duplicateLocationBranch = (await _branchRepository.GetAllAsync())
                                          .FirstOrDefault(b =>
                                              b.Name == branchDto.Name &&
                                              b.Location == branchDto.Location &&
                                              !b.IsDeleted);

            if (duplicateLocationBranch != null)
            {
                return Conflict("A branch with the same name and location already exists.");
            }

            var duplicateMobileOnlyBranch = (await _branchRepository.GetAllAsync())
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
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            };

            await _branchRepository.AddAsync(branch);
            _branchRepository.SaveDB();
            
            var createdBranch = await _branchRepository.GetByIdAsync(branch.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdBranch.Id }, createdBranch);

            //    return CreatedAtAction(nameof(GetById), new { id = branch.Id }, branchDto);
        }

        // Update Branch by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchDTO updatedBranchDto)
        {
            var existingBranch = await _branchRepository.GetByIdAsync(id);
            if (existingBranch == null) return NotFound();

            existingBranch.Name = updatedBranchDto.Name;
            existingBranch.Mobile = updatedBranchDto.Mobile;
            existingBranch.Location = updatedBranchDto.Location;
            existingBranch.CreatedDate = updatedBranchDto.CreatedDate;
            existingBranch.IsDeleted = updatedBranchDto.IsDeleted;


            _branchRepository.Update(existingBranch);
            _branchRepository.SaveDB();

            return Ok(new { message = "The branch has been updated successfully", branch = updatedBranchDto });
        }








        // Soft Delete Branch
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null) return NotFound();

            branch.IsDeleted = true;
            _branchRepository.Update(branch);
            _branchRepository.SaveDB();

            return Ok(new { message = "Branch Successfully Deleted " });
        }
    }
}