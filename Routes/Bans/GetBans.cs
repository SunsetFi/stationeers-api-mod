
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Bans
{
    class GetBans : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "bans" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);
            var bans = await Dispatcher.RunOnMainThread(() =>
            {
                var blocks = Reflection.GetPrivateField<Dictionary<ulong, string>>(BlockedPlayerManager.Instance, "Blocked");
                var banPayloads = new List<BanPayload>();
                foreach (var item in blocks)
                {
                    banPayloads.Add(new BanPayload()
                    {
                        steamId = item.Key.ToString(),
                        endTimestamp = long.Parse(item.Value)
                    });
                }
                return banPayloads;
            });
            await context.SendResponse(HttpStatusCode.OK, bans);
        }
    }
}