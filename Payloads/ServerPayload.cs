
using WebAPI.Models;

namespace WebAPI.Payloads
{
    public class ServerPayload
    {
        public string name { get; set; }
        public string mapName { get; set; }
        public int? maxPlayers { get; set; }
        public string password { get; set; }
        public string startingCondition { get; set; }
        public string respawnCondition { get; set; }

        public static ServerPayload FromServer()
        {
            var payload = new ServerPayload()
            {
                name = ServerModel.Name,
                mapName = ServerModel.MapName,
                maxPlayers = ServerModel.MaxPlayers,
                password = ServerModel.Password,
                startingCondition = ServerModel.StartingCondition,
                respawnCondition = ServerModel.RespawnCondition
            };
            return payload;
        }
    }
}