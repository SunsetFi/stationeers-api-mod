
using System.Collections.Generic;
using Assets.Scripts.Objects.Pipes;

namespace WebAPI.API
{
    public class DeviceItem : ThingItem, ILogicableItem
    {
        public string displayName { get; set; }
        public Dictionary<string, double> logicValues { get; set; }
        public Dictionary<int, Dictionary<string, double>> slotValues { get; set; }

        public static DeviceItem FromDevice(Device device)
        {
            var item = new DeviceItem();
            ThingItem.CopyFromThing(item, device);
            LogicableItemUtils.CopyFromLogicable(item, device);
            return item;
        }
    }
}