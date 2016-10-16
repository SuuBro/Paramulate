using System;
using System.Collections.Generic;
using System.Reflection;

namespace Paramulate.Reflection
{
    internal static class ReflectionUtils
    {
        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static | BindingFlags.Instance
                | BindingFlags.FlattenHierarchy);
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttribute<T>(true) != null;
        }
    }
}