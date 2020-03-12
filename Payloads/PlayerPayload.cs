
using Assets.Scripts;

namespace WebAPI.Payloads
{
    public class PlayerPayload
    {
        public string steamName { get; set; }
        public string steamId { get; set; }

        public static PlayerPayload FromPlayerConnection(PlayerConnection connection)
        {
            var payload = new PlayerPayload();
            payload.steamName = connection.SteamName;
            payload.steamId = connection.SteamId.ToString();
            return payload;
        }
    }
}