
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;

namespace WebAPI.Routes.Atmospheres
{
    class GetAtmospheres : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "atmospheres" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);
            var atmos = await Dispatcher.RunOnMainThread(() => AtmospheresModel.GetAtmospheres());
            await context.SendResponse(HttpStatusCode.OK, atmos);
        }
    }
}