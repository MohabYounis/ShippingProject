using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shipping.DTOs.AccountDto;
using Shipping.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(JwtOptions jwtOptions,UserManager<ApplicationUser> userManager) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user using their email and password, and returns a JWT access token if valid.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing Email and Password.</param>
        /// <returns>
        /// Returns 200 OK with a JWT token if authentication is successful.
        /// Returns 400 Bad Request if the model state is invalid.
        /// Returns 401 Unauthorized if the user does not exist, is marked as deleted, or the password is incorrect.
        /// </returns>

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // البحث عن المستخدم عبر البريد الإلكتروني
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Email or Password" });
            }
            if (user.IsDeleted)
            {
                return Unauthorized(new { message = "user Not authorized" });
            }
            // التحقق من كلمة المرور
            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Invalid Email or Password" });
            }
            var roles = await userManager.GetRolesAsync(user); // بجيب الادوار اللي مرتبطه باليوزر 
                                                               // تكوين قائمة Claims
            var authClaims = new List<Claim>
            {
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Name, user.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role)); //  هنا يتم جلب الأدوار من قاعدة البيانات
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Lifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);// token as object
            var accessToken=tokenHandler.WriteToken(token); //هنا بتحولي token من object ل string
            return Ok(accessToken);
        }
    }
}
