using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webOdev.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp-mail.outlook.com"; // SMTP serveri her mail sitesi için farklı
        private readonly int _smtpPort = 587; // SMTP portu yine mail sitene göre değişken
        private readonly string _smtpUser = "emree_al@outlook.com"; 
        private readonly string _smtpPassword = "tymbxfycppoxakff"; 

        public async Task SendEmailAsync(List<string> recipients, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Admin", "emree_al@outlook.com"));

            // E-posta alıcılarını ekliyoruz
            foreach (var recipient in recipients)
            {
                emailMessage.To.Add(new MailboxAddress("", recipient));
            }

            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { TextBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, false);
                    await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını gösterme
                Console.WriteLine($"Email gönderme hatası: {ex.Message}");
            }
        }
    }
}
