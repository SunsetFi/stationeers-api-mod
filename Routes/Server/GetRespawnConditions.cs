
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;

namespace WebAPI.Routes.Server
{
    class GetRespawnConditions : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "server", "respawn-conditions" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => ServerModel.AllRespawnConditions);
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}