
using System.Collections.Generic;

namespace StationeersWebApi.Payloads
{
    public class DeviceQueryPayload : ThingsQueryPayload
    {
        public List<string> cableNetworkIds { get; set; } = new();
    }
}