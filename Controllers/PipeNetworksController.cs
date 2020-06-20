
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
{
    [WebController(Path = "pipe-networks")]
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