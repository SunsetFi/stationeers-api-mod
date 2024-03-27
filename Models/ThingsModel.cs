
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
            return OcclusionManager.AllThings.Keys.Where(x => x.ReferenceId != 0).Select(thing =>
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