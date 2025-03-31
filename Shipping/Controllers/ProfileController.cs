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
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }


        // Get User by ID
        [HttpGet("{id:alpha}")]
        [EndpointSummary("Get the user's data and view it in his profile page")]
        public async Task<ActionResult<GeneralResponse>> GetById(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null) return NotFound(GeneralResponse.Failure("Not Found."));

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

                return Ok(GeneralResponse.Success(profileDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }


        // Upload profile image
        [HttpPost("{id}/upload-profile-image")]
        [EndpointSummary("Upload the profile image and save it in database")]
        public async Task<ActionResult<GeneralResponse>> UploadProfileImage(string id, IFormFile imageFile)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null) return NotFound(GeneralResponse.Failure("Not Found."));
                if (imageFile == null || imageFile.Length == 0) return BadRequest(GeneralResponse.Failure("No image uploaded."));

                // تأكد من وجود مجلد الصور
                var imagesFolderPath = Path.Combine("wwwroot", "images");
                if (!Directory.Exists(imagesFolderPath))
                    Directory.CreateDirectory(imagesFolderPath);

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
                if (!updateResult.Succeeded) return BadRequest(GeneralResponse.Failure("Failed to update user profile image."));

                return Ok(GeneralResponse.Success("Image uploaded successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GeneralResponse.Failure(ex.Message));
            }
        }
    }
}
