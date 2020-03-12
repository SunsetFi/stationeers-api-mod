
using System.Collections.Generic;
using Assets.Scripts.Networking;
using Newtonsoft.Json;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class BanPlayer : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "players", ":steamId", "ban" };

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
            // Player is allowed to not exist.

            BanPlayerPayload payload = null;
            try
            {
                payload = JsonConvert.DeserializeObject<BanPlayerPayload>(e.Body);
            }
            catch
            {
                e.Context.SendResponse(400, new ErrorPayload()
                {
                    message = "Expected body to be BanPlayerPayload."
                });
                return;
            }

            if (player != null)
            {
                NetworkManagerHudOverride.Instance.KickPlayer(player.SteamName, payload.reason);
            }

            BlockedPlayerManager.Instance.SetBanPlayer(steamId, payload.hours, payload.reason.Length > 0 ? payload.reason : "");

            e.Context.SendResponse(200, PlayerPayload.FromPlayerConnection(player));
        }
    }
}