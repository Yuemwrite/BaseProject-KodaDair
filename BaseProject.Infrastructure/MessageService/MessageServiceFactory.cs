using BaseProject.Application.MessageService;
using BaseProject.Infrastructure.Mailing;
using Domain.Enum;
using Microsoft.Extensions.Configuration;

namespace BaseProject.Infrastructure.MessageService;

public class MessageServiceFactory : IMessageServiceFactory
{
    private readonly IConfiguration _configuration;

    public MessageServiceFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IMessageService MessageService(OneTimePasswordChannel channel)
    {
        
        
        return channel switch
        {
            OneTimePasswordChannel.Email => new EmailService(_configuration),
            OneTimePasswordChannel.Sms => new SmsService(),
            _ => throw new Exception("Tanımsız channel.")
        };
    }
}