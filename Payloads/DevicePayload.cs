
using System.Collections.Generic;
using Assets.Scripts.Objects.Pipes;

namespace WebAPI.Payloads
{
    public class DevicePayload : ThingPayload, ILogicablePayload
    {
        public DevicePayload()
        {
            this.type = "device";
        }

        public string displayName { get; set; }
        public Dictionary<string, LogicValuePayload> logicTypes { get; set; }
        public Dictionary<int, Dictionary<string, double>> slotValues { get; set; }

        public static DevicePayload FromDevice(Device device)
        {
            var item = new DevicePayload();
            ThingPayload.CopyFromThing(item, device);
            LogicableItemUtils.CopyFromLogicable(item, device);
            return item;
        }
    }
}