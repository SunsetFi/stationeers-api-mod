
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networking;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class GetServer : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "server" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            var payload = ServerPayload.FromSteamServer(SteamServer.Instance);
            e.Context.SendResponse(200, payload);
        }
    }
}