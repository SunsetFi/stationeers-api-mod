
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Networking;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class GetPlayers : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "players" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            var players = NetworkManagerOverride.PlayerConnections.Select(x => PlayerPayload.FromPlayerConnection(x));
            e.Context.SendResponse(200, players);
        }
    }
}