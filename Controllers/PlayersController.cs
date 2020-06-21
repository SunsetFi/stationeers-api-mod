
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

namespace WebAPI.Controllers
{

    [WebController(Path = "api/players")]
    public class PlayersController
    {

        [WebRouteMethod(Method = "GET")]
        public async Task GetPlayers(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var players = await Dispatcher.RunOnMainThread(() => PlayersModel.GetPlayers());
            await context.SendResponse(HttpStatusCode.OK, players);
        }

        [WebRouteMethod(Method = "POST", Path = ":steamId/kick")]
        public async Task KickPlayer(IHttpContext context, ulong steamId, KickPlayerPayload body)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => PlayersModel.KickPlayer(steamId, body.reason));

            if (payload == null)
            {
                throw new NotFoundException("No player exist by the given SteamID.");
            }

            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "POST", Path = ":steamId/ban")]
        public async Task BanPlayer(IHttpContext context, ulong steamId, BanPlayerPayload body)
        {
            Authenticator.VerifyAuth(context);

            var bannedPlayer = await Dispatcher.RunOnMainThread(() =>
            {
                var player = PlayersModel.GetPlayer(steamId);
                if (player != null)
                {
                    BansModel.AddBan(steamId, body.hours, body.reason);
                }
                return player;
            });

            if (bannedPlayer == null)
            {
                throw new NotFoundException("No player exist by the given SteamID.");
            }

            await context.SendResponse(HttpStatusCode.OK, bannedPlayer);
        }
    }
}