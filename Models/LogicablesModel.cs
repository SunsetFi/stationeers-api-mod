
using System.Collections.Generic;
using Assets.Scripts.Objects.Pipes;
using WebAPI.Payloads;
using Assets.Scripts.Serialization;
using Assets.Scripts.Objects;

namespace WebAPI.Models
{
    public static class LogicablesModel
    {
        public static IDictionary<string, LogicValuePayload> GetLogicStates(long referenceId)
        {
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out Thing thing))
            {
                return null;
            }

            var logicable = thing as ILogicable;
            if (logicable == null)
            {
                return null;
            }

            return LogicableItemUtils.GetLogicValues(logicable);
        }
    }
}