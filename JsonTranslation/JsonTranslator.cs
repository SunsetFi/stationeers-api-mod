
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace StationeersWebApi.JsonTranslation
{
    public static class JsonTranslator
    {
        private static List<IJsonTranslatorStrategy> strategies = new List<IJsonTranslatorStrategy>();

        public static void LoadJsonTranslatorStrategies(Assembly assembly)
        {
            var payloadStrategyInterface = typeof(IJsonTranslatorStrategy);
            var payloadStrategyAttribute = typeof(JsonTranslatorStrategyAttribute);
            var strategies = (from type in assembly.GetTypes()
                              where type.IsClass && type.GetCustomAttribute(payloadStrategyAttribute) != null
                              let strategy = CreateTranslatorStrategy(type)
                              select strategy).ToArray();
            JsonTranslator.strategies.AddRange(strategies);

            Logging.Log($"Loaded {strategies.Length} JSON translator strategies from {assembly.FullName}.");
        }

        private static IJsonTranslatorStrategy CreateTranslatorStrategy(Type type)
        {
            if (type.GetCustomAttribute(typeof(JsonTranslatorTargetAttribute)) != null)
            {
                return AutoJsonTranslatorStrategy.FromInstance(Activator.CreateInstance(type));
            }
            else if (type.GetInterfaces().Contains(typeof(IJsonTranslatorStrategy)))
            {
                return (IJsonTranslatorStrategy)Activator.CreateInstance(type);
            }
            throw new Exception($"Cannot create translation strategy \"{type.FullName}\": Strategy must either be IJsonTranslatorStrategy or have a JsonTranslatorTargetAttribute.");
        }

        public static JObject ObjectToJson(object target)
        {
            var targetType = target.GetType();

            var strategies = (from strategy in JsonTranslator.strategies
                              where strategy.TargetType.IsAssignableFrom(targetType)
                              select strategy).ToArray();

            if (strategies.Length == 0)
            {
                throw new Exception($"No serializer strategies exist for object of type '{targetType.FullName}'.");
            }

            var jObj = new JObject();
            foreach (var strategy in strategies)
            {
                strategy.WriteObjectToJson(target, jObj);
            }

            return jObj;
        }

        public static void ValidateJsonUpdate(JObject payload, object target)
        {
            var targetType = target.GetType();

            var remainingProperties = new HashSet<string>(payload.Properties().Select(x => x.Name));

            var strategies = from strategy in JsonTranslator.strategies
                             where strategy.TargetType.IsAssignableFrom(targetType)
                             select strategy;

            foreach (var strategy in strategies)
            {
                foreach (var supportedProperty in strategy.SupportedProperties)
                {
                    remainingProperties.Remove(supportedProperty);
                }
            }

            if (remainingProperties.Count > 0)
            {
                throw new JsonTranslationException(string.Format(@"Property '{0}' is invalid for object type '{1}'.", remainingProperties.First(), targetType.Name));
            }
        }

        public static void UpdateObjectFromJson(JObject payload, object target)
        {
            ValidateJsonUpdate(payload, target);

            var targetType = target.GetType();
            var strategies = (from strategy in JsonTranslator.strategies
                              where strategy.TargetType.IsAssignableFrom(targetType)
                              select strategy).ToArray();

            if (strategies.Length == 0)
            {
                throw new Exception(string.Format("No serializer strategies exist for object of type '{0}'.", targetType.Name));
            }

            foreach (var strategy in strategies)
            {
                strategy.UpdateObjectFromJson(target, payload);
            }
        }
    }
}