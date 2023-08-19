using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Shared.Attributes;

namespace Shared.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<EnumDisplayNameAttribute>()
            .GetDisplayName();
    }
    public static string GetCustomDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            .GetName();
    }
}