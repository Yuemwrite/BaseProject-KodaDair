using Shared.Attributes;

namespace Domain.Enum;

public enum OneTimePasswordChannel
{
    [EnumDisplayName(DisplayName = "E-Mail" )]
    Email = 1,
    [EnumDisplayName(DisplayName = "Sms" )]
    Sms = 2
}