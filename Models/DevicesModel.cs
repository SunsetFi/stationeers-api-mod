
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects.Pipes;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class DevicesModel
    {
        public static IList<DevicePayload> GetDevices()
        {
            // Devices can have duplicates in this list.
            var set = new HashSet<Device>();
            foreach (var device in Device.AllDevices)
            {
                set.Add(device);
            }
            return set.Select(x => DevicePayload.FromDevice(x)).ToList();
        }

        public static DevicePayload GetDevice(long referenceId)
        {
            var device = Device.AllDevices.Find(x => x.ReferenceId == referenceId);
            if (device == null)
            {
                return null;
            }
            return DevicePayload.FromDevice(device);
        }

        public static DevicePayload UpdateDevice(long referenceId, DevicePayload updates)
        {
            var device = Device.AllDevices.Find(x => x.ReferenceId == referenceId);
            if (device == null)
            {
                return null;
            }

            ThingModel.WriteThingProperties(device, updates);
            return DevicePayload.FromDevice(device);
        }
    }
}