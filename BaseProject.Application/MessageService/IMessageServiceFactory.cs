using Domain.Enum;

namespace BaseProject.Application.MessageService;

public interface IMessageServiceFactory
{
    IMessageService MessageService(OneTimePasswordChannel channel);
}