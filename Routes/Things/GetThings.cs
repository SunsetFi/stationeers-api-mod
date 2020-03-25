
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Serialization;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Things
{
    class GetThings : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "things" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() =>
            {
                return XmlSaveLoad.Referencables.Values.Select(x => ThingPayload.FromThingByType(x));
            });
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}