using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Todo.Api.Services.Interfaces;

namespace Todo.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationEmail(string toEmail, string emailContent)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            using var smtpClient = new SmtpClient
            {
                Host = smtpSettings["Host"],
                Port = int.Parse(smtpSettings["Port"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSSL"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Username"]),
                Subject = "Bekræftelse af registrering",
                Body = emailContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            try
            {
                // Brug await til at sende e-mailen asynkront
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved sending af e-mail: " + ex.Message);
                throw; // Tilføj throw for at sende undtagelsen videre
            }
        }
    }
}
