
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/pipe-networks")]
    class PipeNetworksController
    {

        [WebRouteMethod(Method = "GET")]
        public async Task GetPipeNetworks(IHttpContext context)
        {
            var skip = 0;
            context.QueryString.TryGetValue("skip", out var skipStr);
            if (!string.IsNullOrEmpty(skipStr) && !int.TryParse(skipStr, out skip))
            {
                throw new BadRequestException("Invalid skip value.");
            }

            var take = int.MaxValue;
            context.QueryString.TryGetValue("take", out var takeStr);
            if (!string.IsNullOrEmpty(takeStr) && !int.TryParse(takeStr, out take))
            {
                throw new BadRequestException("Invalid take value.");
            }

            Authenticator.VerifyAuth(context);
            var networks = await Dispatcher.RunOnMainThread(() => PipeNetworkModel.GetPipeNetworks(skip, take));
            await context.SendResponse(HttpStatusCode.OK, networks);
        }
    }
}