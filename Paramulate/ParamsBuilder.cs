using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using ImpromptuInterface;
using Paramulate.Attributes;
using Paramulate.Exceptions;
using Paramulate.Reflection;
using Paramulate.Serialisation;
using Paramulate.ValueProviders;

namespace Paramulate
{
    public interface IParamsBuilder<T> where T : class
    {
        T Build();

        void WriteParams(T builtParamsObject, TextWriter textWriter);
    }

    public sealed class ParamsBuilder<T> : IParamsBuilder<T> where T : class
    {
        private readonly string _rootName;
        private readonly IReadOnlyList<IValueProvider> _valueProviders;

        internal ParamsBuilder(string rootName, IReadOnlyList<IValueProvider> valueProviders, bool throwOnUnrecognisedParameters)
        {
            if (rootName.Contains(Consts.PathSeperator.ToString()))
            {
                throw new ArgumentException(
                    $"RootName cannot contain a '{Consts.PathSeperator}' char, you supplied: {rootName}");
            }

            _rootName = rootName;
            _valueProviders = valueProviders;
            var initResults = _valueProviders.Select(provider => provider.Init(GetKeys(rootName, typeof(T), 0)));
            CheckForUnrecognisedParameters(throwOnUnrecognisedParameters, initResults);
        }

        private static void CheckForUnrecognisedParameters(bool throwOnUnrecognisedArgs, IEnumerable<InitResult> initResults)
        {
            if (!throwOnUnrecognisedArgs)
            {
                return;
            }
            var unrecognisedParameters = initResults.SelectMany(result => result.UnrecognisedParameters).ToArray();
            if (unrecognisedParameters.Any())
            {
                throw new UnrecognisedParameterException(
                    "Unrecognised arguments were provided which can not be set on " +
                    "the Paramulate tree with root type ITestParameterObject. " +
                    $"Unrecognised arguments:{Environment.NewLine}" +
                    string.Join(Environment.NewLine,
                        unrecognisedParameters.Select(up => $"  '{up.Parameter}' from {up.SourceHint}")));
            }
        }

        private static KeyData[] GetKeys(string path, Type type, int depth)
        {
            var result = new Dictionary<string,KeyData>();
            var propertyInfos = ReflectionUtils.GetProperties(type);
            foreach (var property in propertyInfos)
            {
                var propertyPath = path + "." + property.Name;
                if (ReflectionUtils.IsNestedParameterProperty(property))
                {
                    foreach (var key in GetKeys(propertyPath, property.PropertyType, depth + 1))
                    {
                        result.Add(key.FullKey, key);
                    }
                }
                result.Add(propertyPath, new KeyData(property.PropertyType, propertyPath));
                
                if (depth != 0)
                {
                    continue;
                }
                foreach (var attr in ReflectionUtils.GetAttributes<CommandLineAttribute>(property))
                {
                    KeyData key;
                    var absPath = path + "." + property.Name + "." + attr.PathToDeeperKey; 
                    if (attr.PathToDeeperKey == null)
                    {
                        result[propertyPath] = result[propertyPath].WithCommandLine(
                            attr.ReferenceKey, attr.ShortReferenceKey);
                    }
                    else if (!result.TryGetValue(absPath, out key))
                    {
                        throw new InvalidPropertySpecifierException(type, property.Name, attr.PathToDeeperKey,
                            property, type);
                    }
                    else
                    {
                        result[absPath] =
                            key.WithCommandLine(attr.ReferenceKey, attr.ShortReferenceKey);
                    }
                }
            }
            return result.Values.ToArray();
        }

        private static KeyData AddCommandLineKeys(KeyData keyData, CommandLineAttribute attr)
        {
            return keyData.WithCommandLine(attr?.ReferenceKey, attr?.ShortReferenceKey);
        }

        public static IParamsBuilder<T> New(string root, params IValueProvider[] valueProviders)
        {
            return new ParamsBuilder<T>(root, valueProviders, true);
        }

        public T Build()
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;

            obj[Consts.RootNameField] = _rootName;

            FillObjectDefaults(_rootName, typeof(T), obj);
            SetValuesFromProviders(_rootName, typeof(T), obj);

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


