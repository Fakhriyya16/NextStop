
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Service.Configurations;
using Service.Interfaces;


namespace Service
{
    public class SendEmail : ISendEmail
    {
        private readonly EmailSettings _emailSettings;

        public SendEmail(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        public async Task SendEmailAsync(string to, string subject, string messageBody, bool isHtml = false)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("NextStop", _emailSettings.UserName));
            emailMessage.To.Add(new MailboxAddress("NextStop", to));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(isHtml ? "html" : "plain")
            {
                Text = messageBody
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
