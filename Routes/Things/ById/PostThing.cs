
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects;
using Assets.Scripts.Serialization;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;

namespace WebAPI.Routes.Things.ById
{
    // TODO: Move the Thing specific stuff from here to a shared class so non-Devices can use it too.
    class PostThing : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "things", ":thingId" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            // TODO: Return UNPROCESSABLE_ENTITY if thingId invalid.
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

            ThingPayload payload = null;
            try
            {
                payload = context.ParseBody<ThingPayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be ThingPayload."
                });
                return;
            }

            await Dispatcher.RunOnMainThread(() =>
            {
                ThingModel.WriteThingProperties(thing, payload);
            });

            await context.SendResponse(HttpStatusCode.OK, ThingPayload.FromThingByType(thing));
        }
    }
}