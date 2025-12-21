using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shipping.DTOs.AccountDto;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Services.ModelService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(JwtOptions jwtOptions, UserManager<ApplicationUser> userManager, IEmailSender emailSender,IResetTokenService resetTokenService) : ControllerBase
    {

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
            var accessToken = tokenHandler.WriteToken(token); //هنا بتحولي token من object ل string
            return Ok(accessToken);
        }

        [Authorize(AuthenticationSchemes = "ResetToken")]
        [HttpPost("reset-password-session")]
        public async Task<IActionResult> ResetPasswordSession([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // جاي من التوكين نفسه
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = "Invalid reset token." });

            var user = await userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return Unauthorized(new { message = "Invalid reset token." });

            // نستخدم Identity الرسمي لتغيير الباسورد
            var identityResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, identityResetToken, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new
                {
                    message = "Reset password failed.",
                    errors = result.Errors.Select(e => e.Description)
                });

            return Ok(new { message = "Password has been reset successfully." });
        }

        [HttpPost("forgot-password-otp")]
        public async Task<IActionResult> SendForgotPasswordOtp(
        [FromBody] SendOtpDto dto,[FromServices] ShippingContext db)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(dto.Email);

        // Security: نفس الرد سواء موجود أو لا
        if (user == null || user.IsDeleted)
            return Ok(new { message = "If the email exists, a code was sent." });

        // 1) Invalidate old active OTPs (اختياري لكن مفيد)
        var now = DateTime.UtcNow;
        var actives = await db.PasswordResetOtps
            .Where(x => x.UserId == user.Id && !x.IsUsed && x.ExpiresAt > now)
            .ToListAsync();

        foreach (var x in actives)
            x.IsUsed = true;

        // 2) Generate 6-digit code
        var code = Random.Shared.Next(100000, 999999).ToString();

        // 3) Save hashed code
        db.PasswordResetOtps.Add(new PasswordResetOtp
        {
            UserId = user.Id,
            OtpHash = BCrypt.Net.BCrypt.HashPassword(code),
            ExpiresAt = now.AddMinutes(10),
            Attempts = 0,
            IsUsed = false,
            CreatedAt = now,
            Email= user.Email
        });

        await db.SaveChangesAsync();

        // 4) Send email
            await emailSender.SendAsync(
                user.Email!,
                "Your password reset code",
                $"""
                <p>Your password reset code is:</p>
                <h2 style="letter-spacing:2px;">{code}</h2>
                <p>This code expires in <b>10 minutes</b>.</p>
                """
            );

        return Ok(new { message = "If the email exists, a code was sent." });
    }

        [HttpPost("verify-forgot-password-otp")]
        public async Task<IActionResult> VerifyForgotPasswordOtp(
        [FromBody] VerifyOtpDto dto,[FromServices] ShippingContext db)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.IsDeleted)
                return BadRequest(new { message = "Invalid code." });

            var now = DateTime.UtcNow;

            // آخر OTP active
            var otp = await db.PasswordResetOtps
                .Where(x => x.UserId == user.Id && !x.IsUsed && x.ExpiresAt > now)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (otp == null)
                return BadRequest(new { message = "Invalid or expired code." });

            // attempts limit
            otp.Attempts++;
            if (otp.Attempts > 5)
            {
                otp.IsUsed = true;
                await db.SaveChangesAsync();
                return BadRequest(new { message = "Too many attempts. Please request a new code." });
            }

            var ok = BCrypt.Net.BCrypt.Verify(dto.Code, otp.OtpHash);
            if (!ok)
            {
                await db.SaveChangesAsync();
                return BadRequest(new { message = "Invalid or expired code." });
            }

            // OTP صحيح
            otp.IsUsed = true;
            await db.SaveChangesAsync();

            // 🔑 توليد Reset Session Token
            var resetToken = resetTokenService.GenerateResetToken(user.Id);

            // 👇 ترجعيه في الـ response
            return Ok(new
            {
                message = "OTP verified",
                resetSessionToken = resetToken
            });

        }



    }
}
