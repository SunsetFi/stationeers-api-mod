
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects;
using Newtonsoft.Json.Linq;
using StationeersWebApi.JsonTranslation;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Models
{
    public static class ThingsModel
    {
        public static IList<JObject> GetThings(string prefabName = null, string prefabHashStr = null)
        {
            long prefabHash = 0;
            if (prefabHashStr != null)
            {
                if (!long.TryParse(prefabHashStr, out prefabHash))
                {
                    throw new BadRequestException("Invalid prefabHash.");
                }
            }

            var found = from thing in Thing.AllThings
                            // This seems to have prefabs in it.  Not sure how to filter those out apart from checking ReferenceId 0
                        where thing.ReferenceId != 0
                        where prefabName == null || thing.PrefabName == prefabName
                        where prefabHash == 0 || thing.PrefabHash == prefabHash
                        select JsonTranslator.ObjectToJson(thing);
            return found.ToList();
        }

        public static JObject GetThing(long referenceId)
        {
            if (referenceId == 0)
            {
                return null;
            }

            if (!Thing.TryFind(referenceId, out var thing))
            {
                return null;
            }

            return JsonTranslator.ObjectToJson(thing);
        }

        public static JObject UpdateThing(long referenceId, JObject updates)
        {
            if (!Thing.TryFind(referenceId, out var thing))
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