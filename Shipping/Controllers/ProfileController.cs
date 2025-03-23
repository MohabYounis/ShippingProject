using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.DTOs.Branch;
using Shipping.DTOs;
using Shipping.Services.ModelService;
using Shipping.Models;
using Shipping.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GeneralResponse response;

        public ProfileController(UserManager<ApplicationUser> userManager, GeneralResponse response)
        {
            this.userManager = userManager;
            this.response = response;
        }


        // Get User by ID
        [HttpGet("{id:string}")]
        [EndpointSummary("Get the user's data and view it in his profile page")]
        public async Task<ActionResult<GeneralResponse>> GetById(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Data = "Not Found";
                    return NotFound(response);
                }

                var role = await userManager.GetRolesAsync(user);
                ProfileDto profileDto = new ProfileDto()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = role.ToList(),
                    Address = user.Address,
                    CreatedDate = (user.CreatedDate)?.ToString("dd MMM yyyy"),
                    ProfileImagePath = user.ProfileImagePath ?? string.Empty,
                };

                response.IsSuccess = true;
                response.Data = profileDto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }
            

        // Upload profile image
        [HttpPost("{id:string}/upload-profile-image")]
        [EndpointSummary("Upload the profile image and save it in database")]
        public async Task<ActionResult<GeneralResponse>> UploadProfileImage(string id, IFormFile imageFile)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Data = "User not found.";
                    return NotFound(response);
                }

                if (imageFile == null || imageFile.Length == 0)
                {
                    response.IsSuccess = false;
                    response.Data = "No image uploaded.";
                    return BadRequest(response);
                }

                // تأكد من وجود مجلد الصور
                var imagesFolderPath = Path.Combine("wwwroot", "images");
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                // رفع الصورة وحفظها
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(imagesFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // تحديث المسار في قاعدة البيانات
                user.ProfileImagePath = $"/images/{fileName}";
                var updateResult = await userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Data = "Failed to update user profile image.";
                    return BadRequest(response);
                }

                response.IsSuccess = true;
                response.Data = "Image uploaded successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
