namespace Shipping.DTOs.AccountDto
{
    public class VerifyOtpDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty; // 6 digits
    }
}
