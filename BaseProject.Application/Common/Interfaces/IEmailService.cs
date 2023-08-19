namespace BaseProject.Application.Common.Interfaces;

public interface IEmailService
{
    public bool SendWithSmtp(string to, string subject, string message);
}