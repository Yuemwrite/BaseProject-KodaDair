using Shared.Attributes;

namespace Domain.Enum;

public enum EducationLevel
{
    [EnumDisplayName(DisplayName = "İlkokul" )]
    PrimarySchool = 1,
    [EnumDisplayName(DisplayName = "Ortaokul" )]
    MiddleSchool = 2,
    [EnumDisplayName(DisplayName = "Lise" )]
    HighSchool = 4,
    [EnumDisplayName(DisplayName = "Üniversite" )]
    University = 5,
}