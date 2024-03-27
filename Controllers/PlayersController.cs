
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
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

        [WebRouteMethod(Method = "POST", Path = ":clientId/kick")]
        public async Task KickPlayer(IHttpContext context, ulong clientId)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => PlayersModel.KickPlayer(clientId));

            if (payload == null)
            {
                throw new NotFoundException("No player exist by the given client id.");
            }

            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "POST", Path = ":clientId/ban")]
        public async Task BanPlayer(IHttpContext context, ulong clientId)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => PlayersModel.BanPlayer(clientId));

            if (payload == null)
            {
                throw new NotFoundException("No player exist by the given client id.");
            }

            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}