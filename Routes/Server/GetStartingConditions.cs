
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;

namespace WebAPI.Routes.Server
{
    class GetStartingConditions : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "server", "starting-conditions" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => ServerModel.AllStartingConditions);
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}