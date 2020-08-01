
using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json.Linq;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

namespace WebAPI.Controllers
{
    [WebController(Path = "api/devices")]
    public class DevicesController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetDevices(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var devices = await Dispatcher.RunOnMainThread(() => DevicesModel.GetDevices());
            await context.SendResponse(HttpStatusCode.OK, devices);
        }

        [WebRouteMethod(Method = "GET", Path = ":referenceId")]
        public async Task GetDevice(IHttpContext context, long referenceId)
        {
            Authenticator.VerifyAuth(context);

            var device = await Dispatcher.RunOnMainThread(() => DevicesModel.GetDevice(referenceId));
            if (device == null)
            {
                throw new NotFoundException("Device not found.");
            }

            await context.SendResponse(HttpStatusCode.OK, device);
        }

        [WebRouteMethod(Method = "POST", Path = ":referenceId")]
        public async Task PostDevice(IHttpContext context, long referenceId, JObject body)
        {
            Authenticator.VerifyAuth(context);

            var response = await Dispatcher.RunOnMainThread(() => DevicesModel.UpdateDevice(referenceId, body));

            if (response == null)
            {
                throw new NotFoundException("Device Not Found.");
            }

            await context.SendResponse(HttpStatusCode.OK, response);
        }
    }
}