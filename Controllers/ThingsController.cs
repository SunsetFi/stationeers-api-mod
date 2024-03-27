
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/things")]
    public class ThingsController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetAllThings(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => ThingsModel.GetThings());

            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "GET", Path = ":referenceId")]
        public async Task GetThing(IHttpContext context, long referenceId)
        {
            Authenticator.VerifyAuth(context);

            var thing = await Dispatcher.RunOnMainThread(() => ThingsModel.GetThing(referenceId));
            if (thing == null)
            {
                throw new NotFoundException("Thing not found.");
            }

            await context.SendResponse(HttpStatusCode.OK, thing);
        }

        [WebRouteMethod(Method = "POST", Path = ":referenceId")]
        public async Task PostThing(IHttpContext context, long referenceId, JObject body)
        {
            Authenticator.VerifyAuth(context);

            var response = await Dispatcher.RunOnMainThread(() => ThingsModel.UpdateThing(referenceId, body));

            if (response == null)
            {
                throw new NotFoundException("Thing Not Found.");
            }

            await context.SendResponse(HttpStatusCode.OK, response);
        }
    }
}