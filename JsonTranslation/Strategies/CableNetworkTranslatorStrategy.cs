
using System.Linq;
using Assets.Scripts.Networks;
using Assets.Scripts.Objects.Pipes;

namespace StationeersWebApi.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(Device))]
    public sealed class CableNetworkJsonTranslatorStrategy
    {
        [JsonPropertyGetter("referenceId")]
        public string GetDataNetworkId(CableNetwork network)
        {
            return network.ReferenceId.ToString();
        }

        [JsonPropertyGetter("connectedDeviceReferenceIds")]
        public string[] GetConnectedDevices(CableNetwork network)
        {
            return network.DeviceList.Select(x => x.ReferenceId.ToString()).ToArray();
        }
    }
}