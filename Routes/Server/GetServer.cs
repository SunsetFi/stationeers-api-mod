
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Server
{
    class GetServer : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "server" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => ServerPayload.FromSteamServer(SteamServer.Instance));
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}