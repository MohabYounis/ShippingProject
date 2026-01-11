
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Shipping.Configration;
using Shipping.Services.IModelService;
using System.Net;

namespace Shipping.Services.ModelService
{
    public class SmtpEmailSender:IEmailSender
    {
        private readonly SmtpOptions _opt;
        private readonly ILogger<SmtpEmailSender> _logger;
        public SmtpEmailSender(IOptions<SmtpOptions> opt) => _opt = opt.Value;

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_opt.FromName, _opt.From));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var smtp = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput()));
            // أو بدل Console: ProtocolLogger("smtp.log") لو عايزة ملف

            // ✅ تشخيص: لو في TLS inspection هيبان هنا
            await smtp.ConnectAsync(_opt.Host, _opt.Port, SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_opt.User, _opt.Pass);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
