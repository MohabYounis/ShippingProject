using System.ComponentModel.DataAnnotations;

namespace Shipping.Models
{
    public class PasswordResetOtp
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string OtpHash { get; set; } = default!;
        public DateTimeOffset ExpiresAt { get; set; }

        public int Attempts { get; set; }
        public bool IsUsed { get; set; }

        public string? ResetSessionToken { get; set; } // يتولد بعد Verify
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    }
}
