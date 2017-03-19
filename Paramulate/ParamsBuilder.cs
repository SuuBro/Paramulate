using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using ImpromptuInterface;
using Paramulate.Attributes;
using Paramulate.Reflection;
using Paramulate.Serialisation;
using Paramulate.ValueProviders;

namespace Paramulate
{
    public interface IParamsBuilder<T> where T : class
    {
        T Build(string rootName);

        void WriteParams(T builtParamsObject, TextWriter textWriter);
    }

    public sealed class ParamsBuilder<T> : IParamsBuilder<T> where T : class
    {
        private readonly IReadOnlyList<IValueProvider> _valueProviders;

        internal ParamsBuilder(IReadOnlyList<IValueProvider> valueProviders)
        {
            _valueProviders = valueProviders;
            foreach (var provider in _valueProviders)
            {
                provider.Init(GetKeys());
            }
        }

        private static KeyData[] GetKeys()
        {
            throw new NotImplementedException();
        }

        public static IParamsBuilder<T> New(IReadOnlyList<IValueProvider> valueProviders=null)
        {
            return new ParamsBuilder<T>(valueProviders ?? new IValueProvider[0]);
        }

        public T Build(string rootName)
        {
            if (rootName.Contains(Consts.PathSeperator.ToString()))
            {
                throw new ArgumentException(
                    $"RootName cannot contain a '{Consts.PathSeperator}' char, you supplied: {rootName}");
            }

            var obj = new ExpandoObject() as IDictionary<string, object>;

            obj[Consts.RootNameField] = rootName;

            FillObjectDefaults(rootName, typeof(T), obj);
            SetValuesFromProviders(rootName, typeof(T), obj);

            return obj.ActLike<T>();
        }

        private void SetValuesFromProviders(string path, Type type, IDictionary<string, object> obj)
        {
            var propertyInfos = ReflectionUtils.GetProperties(type);
            foreach (var property in propertyInfos)
            {
                var propertyPath = path + "." + property.Name;

                if (ReflectionUtils.IsNestedParameterProperty(property))
                {
                    SetValuesFromProviders(propertyPath, property.PropertyType,
                                           obj[property.Name] as IDictionary<string, object>);
                    continue;
                }

                Value? value = null;
                foreach (var provider in _valueProviders)
                {
                    value = provider.GetValue(propertyPath);
                    if (value != null)
                    {
                        break;
                    }
                }

                if (value == null)
                {
                    continue;
                }
                
                obj[property.Name] = ValueDeserialiser.GetValue(value.Value.ValueToSet, property.PropertyType,
                    property.Name, "setting value from value provider");
                obj[property.Name + Consts.SourceMetadata] = value.Value.SourceHint;
            }
        }

        public void WriteParams(T builtParamsObject, TextWriter writer)
        {
            var obj = builtParamsObject.UndoActLike() as IDictionary<string, object>;
            if (obj == null)
            {
                throw new ArgumentException("Are you sure you passed in a Paramulate object?");
            }
            writer.WriteLine(obj[Consts.RootNameField]+":");
            PrintParamsObject(1, writer, typeof(T), obj);
        }

        private static string Indent(int depth)
        {
            return new string(' ', depth * 2);
        }

        private static void PrintParamsObject(int depth, TextWriter writer, Type type, IDictionary<string, object> obj)
        {
            var propertyInfos = ReflectionUtils.GetProperties(type);
            foreach (var property in propertyInfos)
            {
                if (ReflectionUtils.IsNestedParameterProperty(property))
                {
                    writer.WriteLine($"{Indent(depth)}{property.Name}:");
                    PrintParamsObject(depth+1, writer, property.PropertyType,
                                      obj[property.Name] as IDictionary<string, object>);
                    writer.WriteLine();
                    continue;
                }

                writer.WriteLine($"{Indent(depth)}{property.Name}:" +
                                 $" {ValueSerialser.Serialize(obj[property.Name])}" +
                                 $" (From {obj[property.Name + Consts.SourceMetadata]})");
            }
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
                obj.Add(property.Name+Consts.SourceMetadata, "Default");
            }
        }
    }
}


