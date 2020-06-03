using System.Collections.Generic;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using System.Threading.Tasks;

namespace WebAPI.Routes.Stationpedia.Logic.Types
{
    class GetTypes : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "stationpedia", "logic", "types" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {           
            Authenticator.VerifyAuth(context);
            await context.SendResponse(HttpStatusCode.OK, LogicTypesPayload.FromGame());
        }
    }
}