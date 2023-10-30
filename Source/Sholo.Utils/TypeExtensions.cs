using System;

namespace Sholo.Utils;

public static class TypeExtensions
{
    // https://stackoverflow.com/a/8142597/179223
    public static bool IsTypeOf<T>(this Type type)
    {
        return typeof(T).IsAssignableFrom(type);
    }

    public static bool IsTypeOf(this Type type, Type otherType)
    {
        return otherType.IsAssignableFrom(type);
    }

    // https://stackoverflow.com/a/5461399/179223 -> https://stackoverflow.com/a/1075059/1968
    public static bool IsAssignableToGenericType(this Type type, Type genericType)
    {
        var interfaceTypes = type.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            return true;

        Type baseType = type.BaseType;
        if (baseType == null) return false;

        return IsAssignableToGenericType(baseType, genericType);
    }
}
