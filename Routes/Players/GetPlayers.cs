
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Ceen;
using WebAPI.Payloads;

namespace WebAPI.Routes.Players
{
    class GetPlayers : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "players" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            var players = await Dispatcher.RunOnMainThread(() => NetworkManagerOverride.PlayerConnections.Select(x => PlayerPayload.FromPlayerConnection(x)));
            await context.SendResponse(HttpStatusCode.OK, players);
        }
    }
}