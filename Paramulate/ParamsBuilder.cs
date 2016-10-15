using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using ImpromptuInterface;
using Newtonsoft.Json;
using Paramulate.Attributes;

namespace Comparams
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
            var propertyInfos = typeof(T).GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static | BindingFlags.Instance
                | BindingFlags.FlattenHierarchy);

            var obj = new ExpandoObject() as IDictionary<string, object>;

            foreach (var propertyInfo in propertyInfos)
            {
                var attr = propertyInfo.GetCustomAttribute<DefaultAttribute>(true);
                obj.Add(propertyInfo.Name, Convert.ChangeType(JsonConvert.DeserializeObject(attr.Value), propertyInfo.PropertyType));
            }

            return obj.ActLike<T>();
        }
    }
}


