using System;
using System.ComponentModel;
using System.Linq;

namespace Sholo.Utils;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
        => value.GetAttributeValue<DescriptionAttribute, string>(d => d.Description);

    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
        => value.GetAttributeValue<TAttribute, TAttribute>(t => t);

    public static TValue GetAttributeValue<TAttribute, TValue>(this Enum value, Func<TAttribute, TValue> attributeValueSelector)
        where TAttribute : Attribute
        => value.GetType()
            .GetMember(value.ToString())
            .SelectMany(x => x.GetCustomAttributes(typeof(TAttribute), false))
            .Cast<TAttribute>()
            .Select(attributeValueSelector)
            .FirstOrDefault();
}
