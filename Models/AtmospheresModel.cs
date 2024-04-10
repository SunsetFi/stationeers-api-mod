
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Atmospherics;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{
    public static class AtmospheresModel
    {
        public static IList<AtmospherePayload> GetAtmospheres(int skip, int take)
        {
            // WHY DOES THIS HAVE NULL ATMOSPHERES???
            return AtmosphericsManager.AllAtmospheres
                .Where(x => x != null)
                .Skip(skip)
                .Take(take)
                .Select(AtmospherePayload.FromAtmosphere).ToList();
        }

        public static IList<AtmospherePayload> GetRoomAtmospheres(int skip, int take)
        {
            // WHY DOES THIS HAVE NULL ATMOSPHERES???
            return AtmosphericsManager.AllAtmospheres
                .Where(x => x != null && x.Room != null)
                .Skip(skip)
                .Take(take)
                .Select(AtmospherePayload.FromAtmosphere).ToList();
        }

        public static IList<AtmospherePayload> GetNetworkAtmospheres(int skip, int take)
        {
            // WHY DOES THIS HAVE NULL ATMOSPHERES???
            return AtmosphericsManager.AllAtmospheres
                .Where(x => x != null && x.AtmosphericsNetwork != null)
                .Skip(skip)
                .Take(take)
                .Select(AtmospherePayload.FromAtmosphere).ToList();
        }
    }
}