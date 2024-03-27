using System.Collections.Generic;
using Assets.Scripts.Objects.Motherboards;
using System;

namespace StationeersWebApi.Payloads
{
    public class LogicSlotTypesPayload
    {
        public Dictionary<string, int> logicSlotTypes { get; set; }

        public static LogicSlotTypesPayload FromGame()
        {
            var item = new LogicSlotTypesPayload();
            item.logicSlotTypes = new Dictionary<string, int>();

            foreach (LogicSlotType slotType in Enum.GetValues(typeof(LogicSlotType)))
            {
                item.logicSlotTypes.Add(slotType.ToString(), (int)slotType);
            }

            return item;
        }
    }
}