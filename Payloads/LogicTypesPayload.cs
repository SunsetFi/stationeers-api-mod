using System.Collections.Generic;
using Assets.Scripts.Objects.Motherboards;
using System;

namespace StationeersWebApi.Payloads
{
    public class LogicTypesPayload
    {
        public Dictionary<string, int> logicTypes { get; set; }

        public static LogicTypesPayload FromGame()
        {
            var item = new LogicTypesPayload();
            item.logicTypes = new Dictionary<string, int>();

            foreach (LogicType type in Enum.GetValues(typeof(LogicType)))
            {
                item.logicTypes.Add(type.ToString(), (int)type);
            }

            return item;
        }
    }
}