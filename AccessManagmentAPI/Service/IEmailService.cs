using AccessManagmentAPI.Models;

namespace AccessManagmentAPI.Service
{
    public interface IEmailService
    {
       Task SendEmail(MailRequest mailrequest);
    }
}
