
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;
using Newtonsoft.Json.Linq;
using StationeersWebApi.JsonTranslation;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Models
{
    public static class ThingsModel
    {
        public static IList<JObject> GetThings()
        {
            // This seems to have prefabs in it.  Not sure how to filter those out apart from checking ReferenceId 0
            return Thing.AllThings.Where(x => x.ReferenceId != 0).Select(thing =>
            {
                try
                {
                    return JsonTranslator.ObjectToJson(thing);
                }
                catch (Exception ex)
                {
                    Logging.Log("GetThings: Exception deserializing thing id {0}: {1}\n{2}", thing.ReferenceId, ex.Message, ex.StackTrace);
                    return null;
                }
            }).Where(x => x != null).ToList();
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