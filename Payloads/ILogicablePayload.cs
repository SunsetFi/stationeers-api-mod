
using System;
using System.Collections.Generic;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;

namespace StationeersWebApi.Payloads
{
    public interface ILogicablePayload
    {
        string displayName { get; set; }
        Dictionary<string, LogicValuePayload> logicTypes { get; set; }

        Dictionary<int, Dictionary<string, double>> slotValues { get; set; }
    }

    public static class LogicableItemUtils
    {
        public static void CopyFromLogicable(ILogicablePayload item, ILogicable logicable)
        {
            item.displayName = logicable.DisplayName;
            item.logicTypes = LogicableItemUtils.GetLogicValues(logicable);
            item.slotValues = LogicableItemUtils.GetSlotValues(logicable);
        }

        public static Dictionary<int, Dictionary<string, double>> GetSlotValues(ILogicable logicable)
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
            return slots;
        }

        public static Dictionary<string, LogicValuePayload> GetLogicValues(ILogicable logicable)
        {
            var logicValues = new Dictionary<string, LogicValuePayload>();
            foreach (LogicType logicType in Enum.GetValues(typeof(LogicType)))
            {
                if (logicable.CanLogicRead(logicType))
                {
                    logicValues.Add(logicType.ToString(), LogicableItemUtils.GetLogicValue(logicable, logicType));
                }
            }

            return logicValues;
        }

        public static LogicValuePayload GetLogicValue(ILogicable logicable, LogicType logicType)
        {
            var value = logicable.GetLogicValue(logicType);
            var writable = logicable.CanLogicWrite(logicType);
            return new LogicValuePayload()
            {
                value = value,
                writable = writable
            };
        }
    }
}