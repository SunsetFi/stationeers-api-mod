
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects.Pipes;
using Newtonsoft.Json.Linq;
using StationeersWebApi.JsonTranslation;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Models
{
    public static class DevicesModel
    {
        public static IList<JObject> GetDevices(string prefabName = null, string prefabHashStr = null)
        {
            long prefabHash = 0;
            if (prefabHashStr != null)
            {
                if (!long.TryParse(prefabHashStr, out prefabHash))
                {
                    throw new BadRequestException("Invalid prefabHash.");
                }
            }

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