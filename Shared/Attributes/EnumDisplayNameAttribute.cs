using Kros.Extensions;

namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Struct
                | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.ReturnValue | AttributeTargets.Enum)]

public class EnumDisplayNameAttribute : Attribute
{
    public string DisplayName { get; set; }
    public string LocalizationKey { get; set; }

    public string GetDisplayName()
    {
        if (LocalizationKey.IsNullOrWhiteSpace())
            return DisplayName;
        
        return DisplayName;
    }
}