
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Ceen;
using WebAPI.Payloads;

namespace WebAPI.Routes.Players.BySteamId
{
    class BanPlayer : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "players", ":steamId", "ban" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            var steamIdStr = pathParams["steamId"];
            ulong steamId;
            if (!ulong.TryParse(steamIdStr, out steamId))
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "SteamID not found."
                });
                return;
            }

            var player = await Dispatcher.RunOnMainThread(() => NetworkManagerOverride.PlayerConnections.Find(x => x.SteamId == steamId));
            // Player is allowed to not exist.

            BanPlayerPayload payload = null;
            try
            {
                payload = context.ParseBody<BanPlayerPayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be BanPlayerPayload."
                });
                return;
            }

            await Dispatcher.RunOnMainThread(() =>
            {
                if (player != null)
                {
                    NetworkManagerHudOverride.Instance.KickPlayer(player.SteamName, payload.reason);
                }

                BlockedPlayerManager.Instance.SetBanPlayer(steamId, payload.hours, payload.reason.Length > 0 ? payload.reason : "");
            });

            await context.SendResponse(HttpStatusCode.OK);
        }
    }
}