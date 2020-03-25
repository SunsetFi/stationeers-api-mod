
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Things.ById
{
    class GetThing : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "things", ":thingId" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            // TODO: Return UNPROCESSABLE_ENTITY if deviceId invalid.
            var referenceId = long.Parse(pathParams["thingId"]);
            var thing = await Dispatcher.RunOnMainThread(() =>
            {
                Thing value;
                if (!XmlSaveLoad.Referencables.TryGetValue(referenceId, out value))
                {
                    return null;
                }
                return value;
            });

            if (thing == null)
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Thing not found."
                });
                return;
            }
            await context.SendResponse(HttpStatusCode.OK, ThingPayload.FromThingByType(thing));
        }
    }
}