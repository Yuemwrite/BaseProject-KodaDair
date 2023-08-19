using Shared.Attributes;

namespace Domain.Enum;

public enum OneTimePasswordType
{
    [EnumDisplayName(DisplayName = "Kayıt Ol" )]
    Register = 1,
    [EnumDisplayName(DisplayName = "Şifremi Unuttum" )]
    ForgotPassword = 2
}