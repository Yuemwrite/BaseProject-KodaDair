using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.MessageService;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace BaseProject.Application.TaskScheduler.Jobs;

[DisallowConcurrentExecution]
public class SendOneTimePasswordJob : IJob
{
    private readonly IMessageServiceFactory _messageServiceFactory;
    private readonly ITwoFactorAuthenticationRepository _twoFactorAuthenticationRepository;

    public SendOneTimePasswordJob(IMessageServiceFactory messageServiceFactory,
        ITwoFactorAuthenticationRepository twoFactorAuthenticationRepository)
    {
        _messageServiceFactory = messageServiceFactory;
        _twoFactorAuthenticationRepository = twoFactorAuthenticationRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var oneTimePasswords = await
            _twoFactorAuthenticationRepository
                .Query()
                .Where(_ => _.IsSend == false)
                .ToListAsync();

        foreach (var oneTimePassword in oneTimePasswords)
        {
            var factory = _messageServiceFactory.MessageService(oneTimePassword.OneTimePasswordChannel);

            await factory.SendMessage(oneTimePassword.To, Subject(oneTimePassword.OneTimePasswordType),
                oneTimePassword.OneTimePassword);

            oneTimePassword.IsSend = true;
            _twoFactorAuthenticationRepository.Update(oneTimePassword);
            await _twoFactorAuthenticationRepository.SaveChangesAsync();
        }
    }

    string Subject(OneTimePasswordType oneTimePasswordType)
    {
        return oneTimePasswordType switch
        {
            OneTimePasswordType.Register => "Koda Dair - Kayıt Aşamasına Son Adımdasınız!",
            OneTimePasswordType.ForgotPassword => "Koda Dair - Şifremi Unuttum",
            _ => throw new Exception("Tanımsız")
        };
    }
}