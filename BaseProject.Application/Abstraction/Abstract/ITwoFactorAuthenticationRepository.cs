using BaseProject.Application.Abstraction.Base;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;

namespace BaseProject.Application.Abstraction.Abstract;

public interface ITwoFactorAuthenticationRepository : IEntityRepository<TwoFactorAuthenticationTransaction>
{
    Task<OneTimePasswordDto> CreateOtp(Guid userId, OneTimePasswordType oneTimePasswordType, string to, OneTimePasswordChannel channel);

    Task<bool> VerifyOtp(Guid userId, OneTimePasswordType oneTimePasswordType, string oneTimePassword);
}