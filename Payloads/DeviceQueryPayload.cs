
using System.Collections.Generic;

namespace StationeersWebApi.Payloads
{
    public class DeviceQueryPayload : ThingsQueryPayload
    {
        public List<string> dataNetworkIds { get; set; } = new();
    }
}