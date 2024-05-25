
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
        public static IList<JObject> GetDevices(string prefabName = null, long prefabHash = 0)
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

                set.Add(device);
            }
            return set.Select(x => JsonTranslator.ObjectToJson(x)).ToList();
        }

        public static IList<JObject> QueryDevices(DeviceQueryPayload query, IList<Device> source = null)
        {
            if (source == null)
            {
                source = Device.AllDevices;
            }

            var set = new HashSet<Device>();
            foreach (var device in source)
            {
                var hasReferenceId = query.referenceIds.Count == 0 || query.referenceIds.Contains(device.ReferenceId.ToString());
                var hasName = query.prefabNames.Count == 0 || query.prefabNames.Contains(device.PrefabName);
                var hasHash = query.prefabHashes.Count == 0 || query.prefabHashes.Contains(device.PrefabHash);
                var hasDisplayName = query.displayNames.Count == 0 || query.displayNames.Contains(device.DisplayName);
                var hasCableNetwork = query.dataNetworkIds.Count == 0 || (device.DataCableNetwork != null && query.dataNetworkIds.Contains(device.DataCableNetwork.ReferenceId.ToString()));

                if (!hasReferenceId || !hasName || !hasHash || !hasDisplayName || !hasCableNetwork)
                {
                    continue;
                }

                if (query.matchIntersection && !(hasReferenceId && hasName && hasHash && hasDisplayName && hasCableNetwork))
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