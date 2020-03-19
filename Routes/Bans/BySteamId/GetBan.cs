
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Models;
using WebAPI.Payloads;

namespace WebAPI.Routes.Bans.BySteamId
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

            var ban = await Dispatcher.RunOnMainThread(() => BansModel.GetBan(steamId));

            if (ban == null)
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "No ban exists by that SteamID."
                });
                return;
            }

            await context.SendResponse(HttpStatusCode.OK, ban);
        }
    }
}