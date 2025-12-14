using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.IO;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services.Jobs
{
    public class ResetPasswordEmailJob : IResetPasswordEmailJob
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ResetPasswordEmailJob> _logger;

        public ResetPasswordEmailJob(IConfiguration configuration, ILogger<ResetPasswordEmailJob> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [Queue("reset-pass-mail")]
        public void Send(string toEmail, string fullName, string token)
        {
            var baseUrl = _configuration["Smtp:ClientBaseUrl"] ?? "http://localhost:5173";
            var resetPath = _configuration["Smtp:ResetPath"] ?? "/reset-password";
            var link = $"{baseUrl.TrimEnd('/')}{resetPath}?token={token}";
            var subject = "Şifre Sıfırlama";
            var body = $"Merhaba {fullName},\n\nŞifrenizi sıfırlamak için bağlantı:\n{link}\n\nBu bağlantı 30 dakika içinde geçerlidir.";

            var fromAddress = _configuration["Smtp:FromAddress"] ?? "no-reply@localhost";
            var fromName = _configuration["Smtp:FromName"] ?? "WMS";

            var usePickup = bool.TryParse(_configuration["Smtp:UsePickupDirectory"], out var pickup) && pickup;
            var message = new MailMessage(new MailAddress(fromAddress, fromName), new MailAddress(toEmail, fullName))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            if (usePickup)
            {
                var pickupDirConfig = _configuration["Smtp:PickupDirectory"];
                string pickupDir;
                if (string.IsNullOrWhiteSpace(pickupDirConfig) || !Path.IsPathRooted(pickupDirConfig))
                {
                    pickupDir = Path.Combine(AppContext.BaseDirectory, "email_pickup");
                }
                else
                {
                    pickupDir = pickupDirConfig;
                }
                Directory.CreateDirectory(pickupDir);
                using var client = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = pickupDir
                };
                client.Send(message);
                _logger.LogInformation($"Şifre sıfırlama e-postası pickup dizinine bırakıldı: {toEmail}");
            }
            else
            {
                var host = _configuration["Smtp:Host"] ?? "localhost";
                var portStr = _configuration["Smtp:Port"];
                var enableSslStr = _configuration["Smtp:EnableSsl"];
                var username = _configuration["Smtp:Username"] ?? string.Empty;
                var password = _configuration["Smtp:Password"] ?? string.Empty;
                var port = 25;
                int.TryParse(portStr, out port);
                var enableSsl = false;
                bool.TryParse(enableSslStr, out enableSsl);

                using var client = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl,
                    Credentials = string.IsNullOrEmpty(username) ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(username, password)
                };
                client.Send(message);
                _logger.LogInformation($"Şifre sıfırlama e-postası gönderildi: {toEmail}");
            }
        }
    }
}
