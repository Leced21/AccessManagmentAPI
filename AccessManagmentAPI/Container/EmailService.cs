using AccessManagmentAPI.Models;
using AccessManagmentAPI.Service;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace AccessManagmentAPI.Container
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            this._emailSettings = options.Value;
        }
        public async Task SendEmail(MailRequest mailrequest)
        {
            var email=new MimeMessage ();
            email.Sender = MailboxAddress.Parse(_emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.Email));
            email.Subject = mailrequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody=mailrequest.Emailbody;
            email.Body=builder.ToMessageBody();

            using var smptp = new SmtpClient ();
            await smptp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smptp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
            await smptp.SendAsync(email);
            await smptp.DisconnectAsync(true);
        }
    }
}
