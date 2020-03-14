
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Objects;
using Ceen;
using WebAPI.Payloads;

namespace WebAPI.Routes.Items
{
    class GetItems : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "items" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            var payload = await Dispatcher.RunOnMainThread(() => Item.AllItems.Select(x => ItemPayload.FromItem(x)));
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}