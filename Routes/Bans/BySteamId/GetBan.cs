
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Payloads;

namespace WebAPI.Routes.Players.BySteamId
{
    class GetBan : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "bans", ":steamId" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            var steamIdStr = pathParams["steamId"];
            ulong steamId;
            if (!ulong.TryParse(steamIdStr, out steamId))
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Invalid SteamID."
                });
                return;
            }

            var ban = await Dispatcher.RunOnMainThread(() =>
            {
                string banTimeout = null;
                var blocks = Reflection.GetPrivateField<Dictionary<ulong, string>>(BlockedPlayerManager.Instance, "Blocked");
                blocks.TryGetValue(steamId, out banTimeout);
                return banTimeout;
            });

            if (ban == null)
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "No ban exists by that SteamID."
                });
                return;
            }

            BanPayload payload = new BanPayload()
            {
                steamId = steamId.ToString(),
                endTimestamp = long.Parse(ban)
            };

            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}