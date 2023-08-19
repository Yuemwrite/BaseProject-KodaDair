
using System.Net.Mail;
using BaseProject.Application.Common.Interfaces;

using Microsoft.Extensions.Configuration;

using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BaseProject.Infrastructure.Mailing;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly IConfiguration _configuration;


    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _emailConfiguration = _configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
    }

    public bool SendWithSmtp(string to, string subject, string message)
    {
        try
        {
            var client = new System.Net.Mail.SmtpClient(_emailConfiguration.Host, _emailConfiguration.Port);
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_emailConfiguration.Username, _emailConfiguration.Password);
            client.EnableSsl = true;
            
            var mailMessage = new MailMessage(_emailConfiguration.Username, to, subject, message);
            client.Send(mailMessage);
           
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}