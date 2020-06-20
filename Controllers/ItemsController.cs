
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
{
    [WebController(Path = "items")]
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