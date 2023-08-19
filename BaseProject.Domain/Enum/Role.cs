using Shared.Attributes;

namespace Domain.Enum;

public enum Role
{
    [EnumDisplayName(DisplayName = "Admin" )]
    Administrator = 1,
    [EnumDisplayName(DisplayName = "Kullanıcı" )]
    User = 2
}