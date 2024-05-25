
using System.Linq;
using Assets.Scripts.Networks;

namespace StationeersWebApi.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(CableNetwork))]
    public sealed class CableNetworkJsonTranslatorStrategy
    {
        [JsonPropertyGetter("referenceId")]
        public string GetDataNetworkId(CableNetwork network)
        {
            return network.ReferenceId.ToString();
        }

        [JsonPropertyGetter("deviceReferenceIds")]
        public string[] GetDevices(CableNetwork network)
        {
            return network.DeviceList.Select(x => x.ReferenceId.ToString()).ToArray();
        }
    }
}