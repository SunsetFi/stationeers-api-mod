
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Atmospherics;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class AtmospheresModel
    {
        public static IList<AtmospherePayload> GetAtmospheres()
        {
            return AtmosphericsManager.AllAtmospheres.Select(atmosphere => AtmospherePayload.FromAtmosphere(atmosphere)).ToList();
        }
    }
}