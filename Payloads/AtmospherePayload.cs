using System;
using Assets.Scripts.Atmospherics;

namespace StationeersWebApi.Payloads
{
    public class AtmospherePayload : IAtmosphericContentPayload
    {
        // Room IDs are long, which potential consumers of json don't really support
        public string roomId { get; set; }
        public string networkReferenceId { get; set; }
        public bool isGlobal { get; set; }

        public float oxygen { get; set; }
        public float volatiles { get; set; }
        public float water { get; set; }
        public float carbonDioxide { get; set; }
        public float nitrogen { get; set; }
        public float nitrousOxide { get; set; }
        public float pollutant { get; set; }
        public float volume { get; set; }
        public float energy { get; set; }
        public float temperature { get; set; }
        public float pressure { get; set; }

        public static AtmospherePayload FromAtmosphere(Atmosphere atmosphere)
        {
            if (atmosphere == null)
            {
                throw new ArgumentNullException(nameof(atmosphere));
            }

            var payload = new AtmospherePayload()
            {
                // Stringify the long for web json consumption.
                roomId = atmosphere.Room?.RoomId.ToString(),
                // Stringify the long for web json consumption.
                networkReferenceId = atmosphere.AtmosphericsNetwork?.ReferenceId.ToString(),
                isGlobal = atmosphere.IsGlobalAtmosphere,
            };
            payload.CopyFromAtmosphere(atmosphere);
            return payload;
        }
    }
}