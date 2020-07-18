
using WebAPI.Models;

namespace WebAPI.Payloads
{
    public class StatusPayload
    {
        public int playerCount { get; set; }
        public string lastSave { get; set; }
        public string gameStatus { get; set; }

        public static StatusPayload FromServer()
        {
            var payload = new StatusPayload()
            {
                playerCount = PlayersModel.GetPlayers().Count,
                lastSave = SettingsModel.LastSave?.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                gameStatus = SettingsModel.GameStatus.ToString()
            };
            return payload;
        }
    }
}