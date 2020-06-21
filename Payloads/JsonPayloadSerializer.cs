
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using WebAPI.Server.Exceptions;

namespace WebAPI.Payloads
{
    public class JsonPayloadStrategyAttribute : Attribute { }

    public static class JsonPayloadSerializer
    {
        private static List<IJsonPayloadStrategy> strategies = new List<IJsonPayloadStrategy>();

        static JsonPayloadSerializer()
        {
            var assembly = typeof(JsonPayloadSerializer).Assembly;
            JsonPayloadSerializer.LoadJsonPayloadStrategies(assembly);
        }

        public static void LoadJsonPayloadStrategies(Assembly assembly)
        {
            var payloadStrategyInterface = typeof(IJsonPayloadStrategy);
            var payloadStrategyAttribute = typeof(JsonPayloadStrategyAttribute);
            var strategies = from type in assembly.GetTypes()
                             where type.IsClass && type.GetInterfaces().Contains(payloadStrategyInterface) && type.GetCustomAttribute(payloadStrategyAttribute) != null
                             let strategy = (IJsonPayloadStrategy)Activator.CreateInstance(type)
                             select strategy;
            JsonPayloadSerializer.strategies.AddRange(strategies);
        }

        public static JObject ObjectToPayload(object target)
        {
            var targetType = target.GetType();

            var strategies = (from strategy in JsonPayloadSerializer.strategies
                              where strategy.TargetType.IsAssignableFrom(targetType)
                              select strategy).ToArray();

            if (strategies.Length == 0)
            {
                throw new Exception(string.Format("No serializer strategies exist for object of type '{0}'.", targetType.Name));
            }

            var jObj = new JObject();
            foreach (var strategy in strategies)
            {
                strategy.WriteObjectToPayload(target, jObj);
            }

            return jObj;
        }

        public static void ValidateUpdatePayload(JObject payload, object target)
        {
            var targetType = target.GetType();

            var remainingProperties = new HashSet<string>(payload.Properties().Select(x => x.Name));

            var strategies = from strategy in JsonPayloadSerializer.strategies
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
                throw new BadRequestException(string.Format(@"Property '{0}' is invalid for object type '{1}'.", remainingProperties.First(), targetType.Name));
            }
        }

        public static void UpdateObjectFromPayload(JObject payload, object target)
        {
            var targetType = target.GetType();
            var strategies = (from strategy in JsonPayloadSerializer.strategies
                              where strategy.TargetType.IsAssignableFrom(targetType)
                              select strategy).ToArray();

            if (strategies.Length == 0)
            {
                throw new Exception(string.Format("No serializer strategies exist for object of type '{0}'.", targetType.Name));
            }

            foreach (var strategy in strategies)
            {
                strategy.UpdateObjectFromPayload(target, payload);
            }
        }
    }
}