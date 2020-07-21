
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
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