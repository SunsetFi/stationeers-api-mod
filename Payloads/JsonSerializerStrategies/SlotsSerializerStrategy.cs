
using System;
using System.Collections.Generic;
using Assets.Scripts.Objects;
using Newtonsoft.Json.Linq;
using WebAPI.Server.Exceptions;

namespace WebAPI.Payloads.JsonSerializerStrategies
{
    public sealed class SlotSerializerStrategy : IJsonPayloadStrategy
    {
        public Type TargetType => typeof(Thing);

        public string[] SupportedProperties => new[] { "slotReferenceIds" };

        public void UpdateObjectFromPayload(object target, JObject input)
        {
            throw new BadRequestException("slotReferenceIds is read only.");
        }

        public void WriteObjectToPayload(object target, JObject output)
        {
            var thing = (Thing)target;

            var refIds = new Dictionary<int, string>();
            for (var i = 0; i < thing.Slots.Count; i++)
            {
                var slot = thing.Slots[i];
                if (slot.Occupant != null)
                {
                    refIds.Add(i, slot.Occupant.ReferenceId.ToString());
                }
            }

            output["slotReferenceIds"] = JObject.FromObject(refIds);
        }
    }
}