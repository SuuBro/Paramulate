using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Paramulate.Attributes;
using Paramulate.Exceptions;
using Paramulate.Serialisation;

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

        public static IEnumerable<T> GetAttributes<T>(PropertyInfo property) where T : Attribute
        {
            return property.GetCustomAttributes<T>();
        }

        public static bool IsNestedParameterProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsInterface
                   && GetProperties(propertyInfo.PropertyType).Any();
        }

        public static bool HasDefaultAttribute(PropertyInfo propertyInfo, out DefaultAttribute defaultAttribute)
        {
            defaultAttribute = propertyInfo.GetCustomAttribute<DefaultAttribute>(true);
            return defaultAttribute != null;
        }

        public static object MakeDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static void SetValue(IDictionary<string, object> child, OverrideDetails details, string root)
        {
            var path = details.Attribute.Path;
            var pathParts = path.Split(Consts.PathSeperator);
            var obj = child;
            var containingType = details.Property.PropertyType;
            foreach (var pathPart in pathParts.Take(pathParts.Count()-1))
            {
                containingType = GetPropertyType(containingType, pathPart, details);
                obj = obj[pathPart] as IDictionary<string, object>;
                if (obj == null)
                {
                    throw new ProgrammerErrorException("We managed to find the type via reflection, but our " +
                                                       "object holding the data is formed to match.");
                }
            }

            var targetPropertyName = pathParts.Last();
            var targetPropertyType = GetPropertyType(containingType, targetPropertyName, details);
            obj[targetPropertyName] = ValueDeserialiser.GetValue(details.Attribute.Value, targetPropertyType,
                $"{root}.{path}", $"setting override from property '{details.Property.Name}' in " +
                                  $"type '{details.ContainingType}'");
            
            var sourcePath = root.Substring(root.IndexOf('.')+1); // Trim the top root
            obj[targetPropertyName+Consts.SourceMetadata] = $"Override on {sourcePath}";
        }

        private static Type GetPropertyType(Type containingType, string propertyName, OverrideDetails details)
        {
            var property = GetProperties(containingType).SingleOrDefault(p => p.Name == propertyName);
            if (property == null)
            {
                throw new InvalidPropertySpecifierException(containingType, propertyName, details);
            }
            return property.PropertyType;
        }
    }
}