
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects.Pipes;
using Newtonsoft.Json.Linq;
using StationeersWebApi.JsonTranslation;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Models
{
    public static class DevicesModel
    {
        public static IList<JObject> GetDevices(string prefabName = null, long prefabHash = 0, string displayName = null)
        {
            // Devices can have duplicates in this list.
            var set = new HashSet<Device>();
            foreach (var device in Device.AllDevices)
            {
                if (prefabName != null && device.PrefabName != prefabName)
                {
                    continue;
                }

                if (prefabHash != 0 && device.PrefabHash != prefabHash)
                {
                    continue;
                }

                if (displayName != null && device.DisplayName != displayName)
                {
                    continue;
                }

                set.Add(device);
            }
            return set.Select(x => JsonTranslator.ObjectToJson(x)).ToList();
        }

        public static IList<JObject> QueryDevices(ThingsQueryPayload query)
        {
            var set = new HashSet<Device>();
            foreach (var device in Device.AllDevices)
            {
                var hasReferenceId = query.referenceIds.Count == 0 || query.referenceIds.Contains(device.ReferenceId.ToString());
                var hasName = query.prefabNames.Count == 0 || query.prefabNames.Contains(device.PrefabName);
                var hasHash = query.prefabHashes.Count == 0 || query.prefabHashes.Contains(device.PrefabHash);
                var hasDisplayName = query.displayNames.Count == 0 || query.displayNames.Contains(device.DisplayName);

                if (!hasReferenceId || !hasName || !hasHash || !hasDisplayName)
                {
                    continue;
                }

                if (query.matchIntersection && !(hasReferenceId && hasName && hasHash && hasDisplayName))
                {
                    continue;
                }


                set.Add(device);
            }
            return set.Select(x => JsonTranslator.ObjectToJson(x)).ToList();
        }

        public static JObject GetDevice(long referenceId)
        {
            var device = Device.AllDevices.Find(x => x.ReferenceId == referenceId);
            if (device == null)
            {
                return null;
            }
            return JsonTranslator.ObjectToJson(device);
        }

        public static JObject UpdateDevice(long referenceId, JObject updates)
        {
            var device = Device.AllDevices.Find(x => x.ReferenceId == referenceId);
            if (device == null)
            {
                return null;
            }

            try
            {
                JsonTranslator.UpdateObjectFromJson(updates, device);
            }
            catch (JsonTranslationException e)
            {
                throw new BadRequestException(e.Message);
            }
            return JsonTranslator.ObjectToJson(device);
        }
    }
}