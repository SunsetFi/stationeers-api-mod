
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;

namespace WebAPI.Routes.PipeNetworks
{
    class GetPipeNetworks : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "pipe-networks" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);
            var networks = await Dispatcher.RunOnMainThread(() => PipeNetworkModel.GetPipeNetworks());
            await context.SendResponse(HttpStatusCode.OK, networks);
        }
    }
}