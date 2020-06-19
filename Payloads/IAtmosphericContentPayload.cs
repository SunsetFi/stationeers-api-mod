
using Assets.Scripts.Atmospherics;

namespace WebAPI.Payloads
{
    public interface IAtmosphericContentPayload
    {
        float oxygen { get; set; }
        float volatiles { get; set; }
        float water { get; set; }
        float carbonDioxide { get; set; }
        float nitrogen { get; set; }
        float nitrousOxide { get; set; }
        float pollutant { get; set; }
        float volume { get; set; }
        float energy { get; set; }
        float temperature { get; set; }
        float pressure { get; set; }
    }

    public static class IAtmosphericContentPayloadUtils
    {
        public static void CopyFromAtmosphere(this IAtmosphericContentPayload payload, Atmosphere atmosphere)
        {
            var mixture = atmosphere.GasMixture;
            payload.oxygen = mixture.Oxygen.Quantity;
            payload.volatiles = mixture.Volatiles.Quantity;
            payload.water = mixture.Water.Quantity;
            payload.carbonDioxide = mixture.CarbonDioxide.Quantity;
            payload.nitrogen = mixture.Nitrogen.Quantity;
            payload.nitrousOxide = mixture.Nitrogen.Quantity;
            payload.pollutant = mixture.Pollutant.Quantity;
            payload.volume = atmosphere.Volume;
            payload.energy = mixture.TotalEnergy;
            payload.temperature = atmosphere.Temperature;
            payload.pressure = atmosphere.PressureGassesAndLiquidsInPa;
        }
    }
}