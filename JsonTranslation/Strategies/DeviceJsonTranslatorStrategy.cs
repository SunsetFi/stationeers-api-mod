
using System.Linq;
using Assets.Scripts.Objects.Pipes;

namespace StationeersWebApi.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(Device))]
    public sealed class DeviceJsonTranslatorStrategy
    {
        [JsonPropertyGetter("cableNetworks")]
        public string[] GetReferenceId(Device device)
        {
            var networks = from cable in device.AttachedCables
                           select cable.CableNetworkId.ToString();
            return networks.Distinct().ToArray();
        }
    }
}