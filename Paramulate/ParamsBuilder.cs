using System;
using System.Collections.Generic;
using System.Dynamic;
using ImpromptuInterface;
using Paramulate.Attributes;
using Paramulate.Reflection;
using Paramulate.Serialisation;

namespace Paramulate
{
    public interface IParamsBuilder<out T> where T : class
    {
        T Build(string rootName);
    }

    internal class Consts
    {
        internal const char PathSeperator = '.';
    }

    public class ParamsBuilder<T> : IParamsBuilder<T> where T : class
    {
        internal ParamsBuilder()
        {
        }

        public static IParamsBuilder<T> New()
        {
            return new ParamsBuilder<T>();
        }

        public T Build(string rootName)
        {
            if (rootName.Contains(Consts.PathSeperator.ToString()))
            {
                throw new ArgumentException(
                    $"RootName cannot contain a '{Consts.PathSeperator}' char, you supplied: {rootName}");
            }

            var obj = new ExpandoObject() as IDictionary<string, object>;

            FillObjectDefaults(rootName, typeof(T), obj);

            return obj.ActLike<T>();
        }

        private static void FillObjectDefaults(string path, Type type, IDictionary<string, object> obj)
        {
            var propertyInfos = ReflectionUtils.GetProperties(type);

            foreach (var property in propertyInfos)
            {
                if (ReflectionUtils.IsNestedParameterProperty(property))
                {
                    var child = new ExpandoObject() as IDictionary<string, object>;
                    FillObjectDefaults($"{path}.{property.Name}", property.PropertyType, child);
                    obj.Add(property.Name, child);

                    var overrides = ReflectionUtils.GetAttributes<OverrideAttribute>(property);
                    foreach (var overrideAttribute in overrides)
                    {
                        var overrideDetails = new OverrideDetails(overrideAttribute, type, property);
                        ReflectionUtils.SetValue(child, overrideDetails, $"{path}.{property.Name}");
                    }
                    continue;
                }

                DefaultAttribute defaultAttr;
                var value = ReflectionUtils.HasDefaultAttribute(property, out defaultAttr)
                    ? ValueDeserialiser.GetValue(defaultAttr.Value, property.PropertyType, property.Name,
                                                 "setting default value")
                    : ReflectionUtils.MakeDefault(property.PropertyType);

                obj.Add(property.Name, value);
            }
        }
    }
}


