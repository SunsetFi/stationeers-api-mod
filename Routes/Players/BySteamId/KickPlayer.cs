
using System.Collections.Generic;
using Assets.Scripts.Networking;
using Newtonsoft.Json;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class KickPlayer : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "players", ":steamId", "kick" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            var steamIdStr = pathParams["steamId"];
            ulong steamId;
            if (!ulong.TryParse(steamIdStr, out steamId))
            {
                e.Context.SendResponse(404, new ErrorPayload()
                {
                    message = "SteamID not found."
                });
            }

            var player = NetworkManagerOverride.PlayerConnections.Find(x => x.SteamId == steamId);
            if (player == null)
            {
                e.Context.SendResponse(404, new ErrorPayload()
                {
                    message = "SteamID not found."
                });
            }

            KickPlayerPayload payload = null;
            try
            {
                payload = JsonConvert.DeserializeObject<KickPlayerPayload>(e.Body);
            }
            catch
            {
                e.Context.SendResponse(500, new ErrorPayload()
                {
                    message = "Expected body to be KickPlayerPayload."
                });
                return;
            }

            NetworkManagerHudOverride.Instance.KickPlayer(player.SteamName, payload.reason.Length > 0 ? payload.reason : "");

            e.Context.SendResponse(200, PlayerPayload.FromPlayerConnection(player));
        }
    }
}