
using System.Threading.Tasks;
using Ceen;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server.Attributes;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/atmospheres")]
    class AtmospheresController
    {

        [WebRouteMethod(Method = "GET")]
        public async Task GetAtmospheres(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            var atmos = await Dispatcher.RunOnMainThread(() => AtmospheresModel.GetAtmospheres());
            await context.SendResponse(HttpStatusCode.OK, atmos);
        }
    }
}