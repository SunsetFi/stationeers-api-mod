
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class GetItems : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "items" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            var items = Item.AllItems.Select(x => ThingPayload.FromThing(x));
            e.Context.SendResponse(200, items);
        }
    }
}