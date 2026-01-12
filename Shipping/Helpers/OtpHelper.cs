using System.Security.Cryptography;
using System.Text;

namespace Shipping.Helpers
{
    public class OtpHelper
    {
        public static string GenerateOtp6()
        {
            var n = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return n.ToString("D6");
        }

        public static string HashOtp(string otp, string secretKey)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
