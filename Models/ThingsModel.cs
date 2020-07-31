
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

            return uniqueThings.Select(thing => JsonPayloadSerializer.ObjectToPayload(thing)).ToList();
        }

        public static JObject GetThing(long referenceId)
        {
            Thing thing;
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out thing))
            {
                return null;
            }
            return JsonPayloadSerializer.ObjectToPayload(thing);
        }

        public static JObject UpdateThing(long referenceId, JObject updates)
        {
            Thing thing;
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out thing))
            {
                return null;
            }

            JsonPayloadSerializer.UpdateObjectFromPayload(updates, thing);
            return JsonPayloadSerializer.ObjectToPayload(thing);
        }
    }
}