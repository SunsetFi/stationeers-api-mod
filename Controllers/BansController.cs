
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;
using WebAPI.Router.Attributes;

namespace WebAPI.Controllers
{
    [WebController(Path = "bans")]
    class BansController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetBans(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            var bans = await Dispatcher.RunOnMainThread(() => BansModel.GetBans());
            await context.SendResponse(HttpStatusCode.OK, bans);
        }

        [WebRouteMethod(Method = "GET", Path = ":steamId")]
        public async Task GetBan(IHttpContext context, string steamId)
        {
            ulong uSteamId;
            if (!ulong.TryParse(steamId, out uSteamId))
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Invalid SteamID."
                });
                return;
            }

            var ban = await Dispatcher.RunOnMainThread(() => BansModel.GetBan(uSteamId));

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

        [WebRouteMethod(Method = "DELETE", Path = ":steamId")]
        public async Task DeleteBan(IHttpContext context, string steamId)
        {
            ulong uSteamId;
            if (!ulong.TryParse(steamId, out uSteamId))
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Invalid SteamID."
                });
                return;
            }

            var removedBan = await Dispatcher.RunOnMainThread(() => BansModel.RemoveBan(uSteamId));

            if (!removedBan)
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "No ban exists by that SteamID."
                });
                return;
            }

            await context.SendResponse(HttpStatusCode.OK);
        }
    }
}