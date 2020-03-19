
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;

namespace WebAPI.Routes.Bans
{
    class GetBans : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "bans" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);
            var bans = await Dispatcher.RunOnMainThread(() => BansModel.GetBans());
            await context.SendResponse(HttpStatusCode.OK, bans);
        }
    }
}