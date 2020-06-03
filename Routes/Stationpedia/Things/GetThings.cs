using System.Collections.Generic;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using System.Threading.Tasks;

namespace WebAPI.Routes.Stationpedia.Things
{
    class GetThings : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "stationpedia", "things" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {            
            Authenticator.VerifyAuth(context);
            
            var things = await Dispatcher.RunOnMainThread(() => ThingPrefabPayload.FromGame());
            await context.SendResponse(HttpStatusCode.OK, things);
        }
    }
}