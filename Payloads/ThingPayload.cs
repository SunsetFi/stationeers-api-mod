

using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Pipes;

namespace WebAPI.Payloads
{
    public class ThingPayload
    {
        public string type { get; set; } = "thing";

        public string referenceId { get; set; }

        public int prefabHash { get; set; }

        public float health { get; set; }

        public string prefabName { get; set; }

        public string customName { get; set; }

        // Needs to be nullable as ThingPayload is used for POSTs and we need to
        //  be able to avoid setting accessState, as 0 is a valid entry.
        public int? accessState { get; set; }

        public Dictionary<int, string> slotReferenceIds { get; set; }

        public static ThingPayload FromThing(Thing thing)
        {
            var payload = new ThingPayload();
            ThingPayload.CopyFromThing(payload, thing);
            return payload;
        }

        public static ThingPayload FromThingByType(Thing thing)
        {
            if (thing is Device)
            {
                return DevicePayload.FromDevice((Device)thing);
            }
            if (thing is Item)
            {
                return ItemPayload.FromItem((Item)thing);
            }
            return ThingPayload.FromThing(thing);
        }

        public static void CopyFromThing(ThingPayload payload, Thing thing)
        {
            payload.referenceId = thing.ReferenceId.ToString();
            payload.prefabHash = thing.PrefabHash;
            payload.prefabName = thing.PrefabName;
            payload.health = thing.ThingHealth;
            payload.customName = thing.IsCustomName ? thing.CustomName : null;
            payload.accessState = thing.AccessState;
            payload.slotReferenceIds = ThingPayload.GetSlotReferenceIds(thing);
        }

        private static Dictionary<int, string> GetSlotReferenceIds(Thing thing)
        {
            var refIds = new Dictionary<int, string>();
            for (var i = 0; i < thing.Slots.Count; i++)
            {
                var slot = thing.Slots[i];
                var refId = slot.Occupant != null ? slot.Occupant.ReferenceId.ToString() : null;
                refIds.Add(i, refId);
            }
            return refIds;
        }
    }
}