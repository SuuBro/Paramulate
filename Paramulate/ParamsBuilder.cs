using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using ImpromptuInterface;
using Paramulate.Attributes;
using Paramulate.Reflection;
using Paramulate.Serialisation;

namespace Paramulate
{
    public interface IParamsBuilder<out T> where T : class
    {
        T Build();
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

        public T Build()
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;

            FillObjectDefaults(typeof(T), obj);

            return obj.ActLike<T>();
        }

        private static void FillObjectDefaults(Type type, IDictionary<string, object> obj)
        {
            var propertyInfos = ReflectionUtils.GetProperties(type);

            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.HasAttribute<ParamulateAttribute>())
                {
                    var child = new ExpandoObject() as IDictionary<string, object>;
                    FillObjectDefaults(propertyInfo.PropertyType, child);
                    obj.Add(propertyInfo.Name, child);
                    continue;
                }

                var attr = propertyInfo.GetCustomAttribute<DefaultAttribute>(true);

                obj.Add(propertyInfo.Name, ValueDeserialiser.GetValue(
                    attr.Value, propertyInfo.PropertyType, propertyInfo.Name));
            }
        }
    }
}


