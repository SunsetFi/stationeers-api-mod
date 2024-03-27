
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/pipe-networks")]
    class PipeNetworksController
    {

        [WebRouteMethod(Method = "GET")]
        public async Task GetPipeNetworks(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            var networks = await Dispatcher.RunOnMainThread(() => PipeNetworkModel.GetPipeNetworks());
            await context.SendResponse(HttpStatusCode.OK, networks);
        }
    }
}