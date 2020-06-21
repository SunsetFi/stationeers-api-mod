
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;
using Newtonsoft.Json.Linq;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class ThingsModel
    {
        public static IList<JObject> GetThings()
        {
            // Dedup thing list
            var uniqueThings = new HashSet<Thing>();
            foreach (var thing in OcclusionManager.AllThings)
            {
                uniqueThings.Add(thing);
            }

            //return uniqueThings.Select(thing => ThingPayload.FromThing(thing)).ToList();
            return uniqueThings.Select(thing => JsonPayloadSerializer.ObjectToPayload(thing)).ToList();
        }

        public static ThingPayload GetThing(long referenceId)
        {
            Thing thing;
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out thing))
            {
                return null;
            }
            return ThingPayload.FromThing(thing);
        }

        public static ThingPayload UpdateThing(long referenceId, ThingPayload updates)
        {
            Thing thing;
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out thing))
            {
                return null;
            }

            ThingsModel.WriteThingProperties(thing, updates);
            return ThingPayload.FromThing(thing);
        }

        public static void WriteThingProperties(Thing thing, ThingPayload payload)
        {
            if (payload.customName != null && payload.customName.Length > 0)
            {
                thing.CustomName = payload.customName;
                thing.IsCustomName = true;
            }

            if (payload.accessState.HasValue)
            {
                thing.AccessState = payload.accessState.Value;
            }
        }
    }
}