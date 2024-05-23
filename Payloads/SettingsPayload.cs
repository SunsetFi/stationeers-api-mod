
using StationeersWebApi.Models;

namespace StationeersWebApi.Payloads
{
    public class SettingsPayload
    {
        public string name { get; set; }
        public string mapName { get; set; }
        public int? maxPlayers { get; set; }
        public string password { get; set; }

        public static SettingsPayload FromServer()
        {
            var payload = new SettingsPayload()
            {
                name = SettingsModel.Name,
                mapName = SettingsModel.MapName,
                maxPlayers = SettingsModel.MaxPlayers,
                password = SettingsModel.Password,
            };
            return payload;
        }
    }
}