
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

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
        public async Task GetBan(IHttpContext context, ulong steamId)
        {
            var ban = await Dispatcher.RunOnMainThread(() => BansModel.GetBan(steamId));

            if (ban == null)
            {
                throw new NotFoundException("No ban exists by the given SteamID.");
            }

            await context.SendResponse(HttpStatusCode.OK, ban);
        }

        [WebRouteMethod(Method = "DELETE", Path = ":steamId")]
        public async Task DeleteBan(IHttpContext context, ulong steamId)
        {
            var removedBan = await Dispatcher.RunOnMainThread(() => BansModel.RemoveBan(steamId));

            if (!removedBan)
            {
                throw new NotFoundException("No ban exists by the given SteamID.");
            }

            await context.SendResponse(HttpStatusCode.OK);
        }
    }
}