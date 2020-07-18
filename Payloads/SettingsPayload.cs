
using WebAPI.Models;

namespace WebAPI.Payloads
{
    public class SettingsPayload
    {
        public string name { get; set; }
        public string mapName { get; set; }
        public int? maxPlayers { get; set; }
        public string password { get; set; }
        public string startingCondition { get; set; }
        public string respawnCondition { get; set; }
        public string lastSave { get; set; }

        public static SettingsPayload FromServer()
        {
            var payload = new SettingsPayload()
            {
                name = SettingsModel.Name,
                mapName = SettingsModel.MapName,
                maxPlayers = SettingsModel.MaxPlayers,
                password = SettingsModel.Password,
                startingCondition = SettingsModel.StartingCondition,
                respawnCondition = SettingsModel.RespawnCondition,
                lastSave = SettingsModel.LastSave?.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            };
            return payload;
        }
    }
}