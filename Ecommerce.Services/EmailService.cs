using Ecommerce.Core.IRepositories.IService;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        //ctor to create constructor
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task sendEmailAsync(string toemail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Lana Hasan", configuration["EmailSetting:FromEmail"]));
            emailMessage.To.Add(new MailboxAddress("", toemail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message };

            var client = new SmtpClient();
            await client.ConnectAsync(configuration["EmailSetting:SmtpServer"],
                 int.Parse(configuration["EmailSetting:Port"]),
                 bool.Parse(configuration["EmailSetting:UseSSL"]));

            await client.AuthenticateAsync(configuration["EmailSetting:FromEmail"],
                configuration["EmailSetting:Password"]);

            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);

        }
    }
}
