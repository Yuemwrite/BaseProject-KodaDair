using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BaseProject.Application.Common.Exceptions;

public static class Extensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>() ?? default!;
    }
    
    public static string GetDescription(this Enum enumValue)
    {
        object[] attr = enumValue.GetType().GetField(enumValue.ToString())!
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attr.Length > 0)
            return ((DescriptionAttribute)attr[0]).Description;
        string result = enumValue.ToString();
        result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2");
        result = Regex.Replace(result, "([A-Za-z])([0-9])", "$1 $2");
        result = Regex.Replace(result, "([0-9])([A-Za-z])", "$1 $2");
        result = Regex.Replace(result, "(?<!^)(?<! )([A-Z][a-z])", " $1");
        return result;
    }
    
    public static List<string> GetDescriptionList(this Enum enumValue)
    {
        string result = enumValue.GetDescription();
        return result.Split(',').ToList();
    }
}