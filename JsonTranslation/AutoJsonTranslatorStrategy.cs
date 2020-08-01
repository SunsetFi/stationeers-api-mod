using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace WebAPI.JsonTranslation
{
    static class AutoJsonTranslatorStrategy
    {
        public static IJsonTranslatorStrategy FromInstance(object instance)
        {
            var type = instance.GetType();
            var targetAttribute = (JsonTranslatorTargetAttribute)Attribute.GetCustomAttribute(type, typeof(JsonTranslatorTargetAttribute));
            if (targetAttribute == null)
            {
                throw new Exception("Instance passed to AutoJsonTranslatorStrategy must have a JsonTranslatorTargetAttribute.");
            }

            var constructorType = typeof(AutoJsonTranslatorStrategy<>).MakeGenericType(type);
            return (IJsonTranslatorStrategy)Activator.CreateInstance(constructorType, instance);
        }
    }

    class AutoJsonTranslatorStrategy<T> : IJsonTranslatorStrategy
    {
        private static Dictionary<Type, string> sExpectedTypeErrors = new Dictionary<Type, string> {
            {typeof(int), "integer"},
            {typeof(float), "decimal"},
            {typeof(double), "decimal"},
            {typeof(string), "string"},
            {typeof(JObject), "object"},
            {typeof(JToken), "json value"},
        };

        private object instance;

        private HashSet<string> supportedProperties = new HashSet<string>();
        private Dictionary<string, MethodInfo> propertyGetters = new Dictionary<string, MethodInfo>();
        private Dictionary<string, MethodInfo> propertySetters = new Dictionary<string, MethodInfo>();

        public AutoJsonTranslatorStrategy(object instance)
        {
            this.instance = instance;

            var getters =
                from method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                let attribute = (JsonPropertyGetterAttribute)Attribute.GetCustomAttribute(method, typeof(JsonPropertyGetterAttribute))
                where attribute != null
                select new { Method = method, Attribute = attribute };
            foreach (var getter in getters)
            {
                var method = getter.Method;
                var methodParams = method.GetParameters();
                if (methodParams.Length != 1)
                {
                    throw new Exception("Expected getter method to have exactly 1 parameter");
                }
                if (!methodParams[0].ParameterType.IsAssignableFrom(typeof(T)))
                {
                    throw new Exception($"Expected getter method parameter to be assignable from TargetType \"{typeof(T).FullName}\".");
                }

                this.propertyGetters.Add(getter.Attribute.PropertyName, method);
                this.supportedProperties.Add(getter.Attribute.PropertyName);
            }

            var setters =
                from method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                let attribute = (JsonPropertySetterAttribute)Attribute.GetCustomAttribute(method, typeof(JsonPropertySetterAttribute))
                where attribute != null
                select new { Method = method, Attribute = attribute };
            foreach (var setter in setters)
            {
                var method = setter.Method;
                var methodParams = method.GetParameters();
                if (methodParams.Length != 2)
                {
                    throw new Exception("Expected setter method to have exactly 2 parameters");
                }
                if (!methodParams[0].ParameterType.IsAssignableFrom(typeof(T)))
                {
                    throw new Exception($"Expected setter method first parameter to be assignable from TargetType \"{typeof(T).FullName}\".");
                }

                this.propertySetters.Add(setter.Attribute.PropertyName, setter.Method);
                this.supportedProperties.Add(setter.Attribute.PropertyName);
            }
        }

        public Type TargetType => typeof(T);

        public string[] SupportedProperties
        {
            get
            {
                return this.supportedProperties.ToArray();
            }
        }

        public void VerifyJsonUpdate(object target, JObject input)
        {
            foreach (var property in this.supportedProperties)
            {
                if (!input.ContainsKey(property))
                {
                    continue;
                }

                // Do a dry-run of casting the value, to throw exceptions for incompatible types
                this.CastSetterValue(property, input[property]);
            }
        }

        public void UpdateObjectFromJson(object target, JObject input)
        {
            // Handle all properties, so we can generate read only exceptions if needed.
            foreach (var property in this.supportedProperties)
            {
                if (!input.ContainsKey(property))
                {
                    continue;
                }

                var value = this.CastSetterValue(property, input[property]);
                this.propertySetters[property].Invoke(this.instance, new[] { target, value });
            }
        }



        public void WriteObjectToJson(object target, JObject output)
        {
            // We only care about writing properties we can read.  No WriteOnlyExceptions here.
            foreach (var pair in this.propertyGetters)
            {
                var value = pair.Value.Invoke(this.instance, new[] { target });
                output[pair.Key] = (JToken)value;
            }
        }

        private object CastSetterValue(string property, JToken token)
        {
            if (!this.propertySetters.TryGetValue(property, out var setterInfo))
            {
                throw new ReadOnlyPropertyException($"Property \"{property}\" is read-only.");
            }

            var expectedType = setterInfo.GetParameters()[1].ParameterType;

            // Not sure if this null handling is required, token.ToObject might just handle it.
            var underlyingType = Nullable.GetUnderlyingType(expectedType);
            if (underlyingType != null)
            {
                if (token.Type == JTokenType.Null)
                {
                    return null;
                }
                expectedType = underlyingType;
            }
            else if (token.Type == JTokenType.Null)
            {
                throw new PropertyTypeException($"Property \"{property}\" cannot be null.");
            }

            // Try to get the value.
            //  If we fail, try to produce a sensible error message.
            //  Failure is assumed to be due to data type.  We could use a better Exception type here.
            try
            {
                return token.ToObject(expectedType);
            }
            catch (Exception)
            {
                string typeName;
                if (!sExpectedTypeErrors.TryGetValue(expectedType, out typeName))
                {
                    typeName = expectedType.Name;
                }

                throw new PropertyTypeException($"Property \"{property}\" should be of type \"{expectedType.Name}\".");
            }
        }
    }
}