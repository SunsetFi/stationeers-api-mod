

using System;
using System.Collections.Generic;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Newtonsoft.Json.Linq;

namespace WebAPI.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(ILogicable))]
    public sealed class LogicableJsonTranslatorStrategy
    {
        [JsonPropertyGetter("displayName")]
        public string GetDisplayName(ILogicable logicable)
        {
            return logicable.DisplayName;
        }

        [JsonPropertyGetter("logicValues")]
        public JObject GetLogicValues(ILogicable logicable)
        {
            var logicValues = new Dictionary<string, double>();
            foreach (LogicType logicType in Enum.GetValues(typeof(LogicType)))
            {
                if (logicable.CanLogicRead(logicType))
                {
                    var value = logicable.GetLogicValue(logicType);
                    logicValues.Add(logicType.ToString(), value);
                }
            }

            return JObject.FromObject(logicValues);
        }

        [JsonPropertyGetter("logicSlotValues")]
        public JObject GetLogicSlotValues(ILogicable logicable)
        {
            var slots = new Dictionary<int, Dictionary<string, double>>();
            for (var i = 0; i < logicable.TotalSlots; i++)
            {
                var logicValues = new Dictionary<string, double>();
                foreach (LogicSlotType logicType in Enum.GetValues(typeof(LogicSlotType)))
                {
                    if (logicable.CanLogicRead(logicType, i))
                    {
                        var value = logicable.GetLogicValue(logicType, i);
                        logicValues.Add(logicType.ToString(), value);
                    }
                }
                slots.Add(i, logicValues);
            }
            return JObject.FromObject(slots);
        }
    }
}