
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Atmospherics;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{
    public static class AtmospheresModel
    {
        public static IList<AtmospherePayload> GetAtmospheres()
        {
            return AtmosphericsManager.AllAtmospheres.Select(atmosphere => AtmospherePayload.FromAtmosphere(atmosphere)).ToList();
        }
    }
}