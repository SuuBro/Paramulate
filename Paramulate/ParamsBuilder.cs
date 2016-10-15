using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using ImpromptuInterface;
using Newtonsoft.Json;
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
            var propertyInfos = ReflectionUtils.GetProperties<T>();

            var obj = new ExpandoObject() as IDictionary<string, object>;

            foreach (var propertyInfo in propertyInfos)
            {
                var attr = propertyInfo.GetCustomAttribute<DefaultAttribute>(true);

                obj.Add(propertyInfo.Name, ValueDeserialiser.GetValue(
                    attr.Value, propertyInfo.PropertyType, propertyInfo.Name));
            }

            return obj.ActLike<T>();
        }


    }
}


