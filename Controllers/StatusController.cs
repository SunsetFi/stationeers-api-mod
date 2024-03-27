
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/status")]
    public class StatusController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetStatus(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => StatusPayload.FromServer());
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}