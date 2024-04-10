using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networks;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{
    public static class PipeNetworkModel
    {
        public static IList<PipeNetworkPayload> GetPipeNetworks(int skip, int take)
        {
            return PipeNetwork.AllPipeNetworks
                .Where(x => x != null)
                .Skip(skip)
                .Take(take)
                .Select(PipeNetworkPayload.FromPipeNetwork).ToList();
        }
    }
}