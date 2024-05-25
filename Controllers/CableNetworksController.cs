

using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Payloads;
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

        [WebRouteMethod(Method = "GET", Path = ":referenceId")]
        public async Task GetCableNetwork(IHttpContext context, long referenceId)
        {
            Authenticator.VerifyAuth(context);

            var network = await Dispatcher.RunOnMainThread(() => CableNetworksModel.GetCableNetwork(referenceId));
            if (network == null)
            {
                throw new NotFoundException("Cable network not found");
            }

            await context.SendResponse(HttpStatusCode.OK, network);
        }

        [WebRouteMethod(Method = "GET", Path = ":referenceId/devices")]
        public async Task GetCableNetworkDevices(IHttpContext context, long referenceId)
        {
            Authenticator.VerifyAuth(context);

            var devices = await Dispatcher.RunOnMainThread(() => CableNetworksModel.GetCableNetworkDevices(referenceId));
            if (devices == null)
            {
                throw new NotFoundException("Cable network not found");
            }

            await context.SendResponse(HttpStatusCode.OK, devices);
        }

        [WebRouteMethod(Method = "POST", Path = ":referenceId/devices/query")]
        public async Task QueryCableNetworkDevices(IHttpContext context, long referenceId, DeviceQueryPayload query)
        {
            Authenticator.VerifyAuth(context);

            var devices = await Dispatcher.RunOnMainThread(() => CableNetworksModel.QueryCableNetworkDevices(referenceId, query));
            if (devices == null)
            {
                throw new NotFoundException("Cable network not found");
            }

            await context.SendResponse(HttpStatusCode.OK, devices);
        }
    }
}