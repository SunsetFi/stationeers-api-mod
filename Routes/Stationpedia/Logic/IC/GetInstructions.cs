using System.Collections.Generic;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using System.Threading.Tasks;

namespace WebAPI.Routes.Stationpedia.Logic.IC
{
    class GetInstructions : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "stationpedia", "logic", "ic", "instructions" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {            
            Authenticator.VerifyAuth(context);
            await context.SendResponse(HttpStatusCode.OK, ICInstructionPayload.FromGame());
        }
    }
}