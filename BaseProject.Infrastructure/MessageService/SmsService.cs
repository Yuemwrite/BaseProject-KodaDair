using BaseProject.Application.MessageService;

namespace BaseProject.Infrastructure.MessageService;

public class SmsService : IMessageService
{
    public Task<object> SendMessage(string to, string subject, string message)
    {
        throw new NotImplementedException("SMS SERVİSİ AKTİF DEĞİL.");
    }
}