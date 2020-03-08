
using System;
using System.Collections.Generic;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;

namespace WebAPI.API
{
    public interface ILogicableItem
    {
        string displayName { get; set; }
        Dictionary<string, double> logicValues { get; set; }
    }

    public static class LogicableItemUtils
    {
        public static void CopyFromLogicable(ILogicableItem item, ILogicable logicable)
        {
            item.displayName = logicable.DisplayName;
            item.logicValues = LogicableItemUtils.GetLogicValues(logicable);
        }

        public static Dictionary<string, double> GetLogicValues(ILogicable logicable)
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

            return logicValues;
        }
    }
}