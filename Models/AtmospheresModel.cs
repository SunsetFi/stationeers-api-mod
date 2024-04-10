
using System.Collections.Generic;
using Assets.Scripts.Atmospherics;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{
    public static class AtmospheresModel
    {
        public static IList<AtmospherePayload> GetAtmospheres()
        {
            var items = new List<AtmospherePayload>(AtmosphericsManager.AllAtmospheres.Count);
            foreach (var atmo in AtmosphericsManager.AllAtmospheres)
            {
                if (atmo == null)
                {
                    // WHY DOES THIS HAVE NULL ATMOSPHERES???
                    continue;
                }

                var payload = AtmospherePayload.FromAtmosphere(atmo);
                items.Add(payload);
            }

            return items;
        }
    }
}