

using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/cable-networks")]
    public class CableNetworksController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetCableNetworks(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var networks = await Dispatcher.RunOnMainThread(() => CableNetworksModel.GetCableNetworks());
            await context.SendResponse(HttpStatusCode.OK, networks);
        }

        [WebRouteMethod(Method = "GET", Path = "/:referenceId")]
        public async Task GetCableNetwork(IHttpContext context, string referenceId)
        {
            Authenticator.VerifyAuth(context);

            if (!long.TryParse(referenceId, out var referenceIdLong))
            {
                await context.SendResponse(HttpStatusCode.BadRequest, "Invalid referenceId");
                return;
            }

            var network = await Dispatcher.RunOnMainThread(() => CableNetworksModel.GetCableNetwork(referenceIdLong));
            if (network == null)
            {
                throw new NotFoundException("Cable network not found");
            }

            await context.SendResponse(HttpStatusCode.OK, network);
        }

        [WebRouteMethod(Method = "GET", Path = "/:referenceId/devices")]
        public async Task GetCableNetworkDevices(IHttpContext context, string referenceId)
        {
            Authenticator.VerifyAuth(context);

            if (!long.TryParse(referenceId, out var referenceIdLong))
            {
                await context.SendResponse(HttpStatusCode.BadRequest, "Invalid referenceId");
                return;
            }

            var devices = await Dispatcher.RunOnMainThread(() => CableNetworksModel.GetCableNetworkDevices(referenceIdLong));
            if (devices == null)
            {
                throw new NotFoundException("Cable network not found");
            }

            await context.SendResponse(HttpStatusCode.OK, devices);
        }
    }
}