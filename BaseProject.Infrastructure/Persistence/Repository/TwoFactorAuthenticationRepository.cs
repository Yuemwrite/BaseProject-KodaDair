using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Utilities.Encryption;
using BaseProject.Application.Utilities.Toolkit;
using BaseProject.Infrastructure.Context;
using BaseProject.Infrastructure.Mailing;
using BaseProject.Infrastructure.Persistence.Base;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MailKit.AspNet.Core;
using Microsoft.Extensions.Configuration;
using IEmailService = BaseProject.Application.Common.Interfaces.IEmailService;

namespace BaseProject.Infrastructure.Persistence.Repository;

public class TwoFactorAuthenticationRepository :
    EfEntityRepositoryBase<TwoFactorAuthenticationTransaction, ApplicationDbContext>, ITwoFactorAuthenticationRepository
{
    private readonly IEmailService _emailService;
    private readonly EmailConfiguration _emailConfiguration;


    public TwoFactorAuthenticationRepository(ApplicationDbContext context, IEmailService emailService,
        IConfiguration configuration) : base(context)
    {
        _emailService = emailService;
        _emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
    }

    public async Task<OneTimePasswordDto> CreateOtp(Guid userId, OneTimePasswordType oneTimePasswordType, string to,
        OneTimePasswordChannel channel)
    {
        var oneTimePassword = RandomGenerator.RandomOneTimePassword();

        var twoFactorAuthentication = new TwoFactorAuthenticationTransaction()
        {
            UserId = userId,
            OneTimePasswordType = oneTimePasswordType,
            OneTimePasswordChannel = channel,
            To = to,
            OneTimePassword = oneTimePassword,
            IsUsed = false,
            IsSend = false
        };

        Context.Add(twoFactorAuthentication);
        await Context.SaveChangesAsync();
        
        var oneTimePasswordDto = new OneTimePasswordDto()
        {
            Success = true,
            OneTimePassword = (channel == OneTimePasswordChannel.Email && _emailConfiguration.IsEnabled)
                ? null
                : oneTimePassword,
            OneTimePasswordId = twoFactorAuthentication.Id
        };

        return oneTimePasswordDto;
    }

    public async Task<bool> VerifyOtp(Guid userId, OneTimePasswordType oneTimePasswordType, string oneTimePassword)
    {
        
        var oneTimePasswordTransaction = Query()
            .Where(_ => _.UserId == userId && _.OneTimePasswordType == oneTimePasswordType)
            .OrderByDescending(_ => _.Id)
            .AsQueryable();

        var otpTransaction = oneTimePasswordTransaction.FirstOrDefault();

        if (otpTransaction is null)
        {
            return false;
        }

        if (otpTransaction.IsUsed)
        {
            return false;
        }

        if (otpTransaction.OneTimePassword != oneTimePassword)
        {
            return false;
        }

        otpTransaction.IsUsed = true;

        Context.Update(otpTransaction);
        await Context.SaveChangesAsync();

        return true;
    }
}