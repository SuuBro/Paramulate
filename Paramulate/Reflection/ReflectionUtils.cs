using System.Collections.Generic;
using System.Reflection;

namespace Paramulate.Reflection
{
    internal static class ReflectionUtils
    {
        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static | BindingFlags.Instance
                | BindingFlags.FlattenHierarchy);
        }
    }
}