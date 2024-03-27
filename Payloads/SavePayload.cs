
using StationeersWebApi.Models;

namespace StationeersWebApi.Payloads
{
    public class SavePayload
    {
        public string lastSave { get; set; }

        public static SavePayload FromServer()
        {
            var payload = new SavePayload()
            {
                lastSave = SettingsModel.LastSave?.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            };
            return payload;
        }
    }
}