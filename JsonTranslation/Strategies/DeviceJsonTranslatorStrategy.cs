
using Assets.Scripts.Objects.Pipes;

namespace StationeersWebApi.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(Device))]
    public sealed class DeviceJsonTranslatorStrategy
    {
        [JsonPropertyGetter("dataNetworkId")]
        public string GetDataNetworkId(Device device)
        {
            if (device.DataCableNetwork == null)
            {
                return null;
            }

            return device.DataCableNetwork.ReferenceId.ToString();
        }
    }
}