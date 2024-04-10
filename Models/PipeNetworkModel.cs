
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networks;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{
    public static class PipeNetworkModel
    {
        public static IList<PipeNetworkPayload> GetPipeNetworks()
        {
            return PipeNetwork.AllPipeNetworks.Select(PipeNetworkPayload.FromPipeNetwork).ToList();
        }
    }
}