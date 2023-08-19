namespace BaseProject.Application.MessageService;

public interface IMessageService
{
    public Task<object> SendMessage(string to, string subject, string message);
}