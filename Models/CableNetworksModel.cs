using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networks;
using Newtonsoft.Json.Linq;
using StationeersWebApi.JsonTranslation;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{
    public static class CableNetworksModel
    {
        public static IList<JObject> GetCableNetworks()
        {
            return CableNetwork.AllCableNetworks.Select(x => JsonTranslator.ObjectToJson(x)).ToList();
        }

        public static JObject GetCableNetwork(long referenceId)
        {
            var network = CableNetwork.AllCableNetworks.FirstOrDefault(x => x.ReferenceId == referenceId);
            if (network == null)
            {
                return null;
            }

            return JsonTranslator.ObjectToJson(network);
        }

        public static IList<JObject> GetCableNetworkDevices(long referenceId)
        {
            var network = CableNetwork.AllCableNetworks.FirstOrDefault(x => x.ReferenceId == referenceId);
            if (network == null)
            {
                return null;
            }

            return network.DeviceList.Select(x => JsonTranslator.ObjectToJson(x)).ToList();
        }

        public static IList<JObject> QueryCableNetworkDevices(long referenceId, DeviceQueryPayload query)
        {
            var network = CableNetwork.AllCableNetworks.FirstOrDefault(x => x.ReferenceId == referenceId);
            if (network == null)
            {
                return null;
            }

            return DevicesModel.QueryDevices(query, network.DeviceList);
        }
    }
}