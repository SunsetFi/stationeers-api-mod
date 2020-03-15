
using Assets.Scripts.Networking;

namespace WebAPI.Payloads
{
    public class ServerPayload
    {
        public string name { get; set; }
        public string mapName { get; set; }
        public int maxPlayers { get; set; }
        public string password { get; set; }

        public static ServerPayload FromSteamServer(SteamServer server)
        {
            var payload = new ServerPayload()
            {
                name = Reflection.GetPrivateField<string>(server, "ServerNameText"),
                mapName = WorldManager.CurrentWorldName, // server.MapName.value
                maxPlayers = int.Parse(Reflection.GetPrivateField<string>(server, "MaxPlayerText")),
                password = Reflection.GetPrivateField<string>(server, "PasswordText")
            };
            return payload;
        }
    }
}