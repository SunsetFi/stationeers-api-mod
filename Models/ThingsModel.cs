
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;
using Newtonsoft.Json.Linq;
using WebAPI.JsonTranslation;
using WebAPI.Server.Exceptions;

namespace WebAPI.Models
{
    public static class ThingsModel
    {
        public static IList<JObject> GetThings()
        {
            // Dedup thing list
            var uniqueThings = new HashSet<Thing>();
            foreach (var thing in OcclusionManager.AllThings.Keys)
            {
                uniqueThings.Add(thing);
            }

            return uniqueThings.Select(thing => JsonTranslator.ObjectToJson(thing)).ToList();
        }

        public static JObject GetThing(long referenceId)
        {
            Thing thing;
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out thing))
            {
                return null;
            }
            return JsonTranslator.ObjectToJson(thing);
        }

        public static JObject UpdateThing(long referenceId, JObject updates)
        {
            Thing thing;
            if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out thing))
            {
                return null;
            }

            try
            {
                JsonTranslator.UpdateObjectFromJson(updates, thing);
            }
            catch (JsonTranslationException e)
            {
                throw new BadRequestException(e.Message);
            }
            return JsonTranslator.ObjectToJson(thing);
        }
    }
}