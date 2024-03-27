
using System.Threading.Tasks;
using Ceen;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Server.Attributes;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/items")]
    class ItemsController
    {

        [WebRouteMethod(Method = "GET")]
        public async Task GetItems(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            var items = await Dispatcher.RunOnMainThread(() => ItemsModel.GetItems());
            await context.SendResponse(HttpStatusCode.OK, items);
        }
    }
}