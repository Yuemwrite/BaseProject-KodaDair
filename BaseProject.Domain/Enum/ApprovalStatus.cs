using Shared.Attributes;

namespace Domain.Enum;

public enum ApprovalStatus
{
    [EnumDisplayName(DisplayName = "Bekliyor" )]
    Pending = 1,
    [EnumDisplayName(DisplayName = "Onaylandı" )]
    Approved = 2,
    [EnumDisplayName(DisplayName = "Reddedildi" )]
    Rejected = 3,
    [EnumDisplayName(DisplayName = "Takipten Çıktı" )]
    Unfollow = 4,
}