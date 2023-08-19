using Shared.Attributes;

namespace Domain.Enum;

public enum SharingType
{
    [EnumDisplayName(DisplayName = "Soru" )]
    Question = 1, 
    [EnumDisplayName(DisplayName = "Anket" )]
    Survey = 2, 
}