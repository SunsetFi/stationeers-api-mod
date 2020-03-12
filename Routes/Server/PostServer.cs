
using System.Collections.Generic;
using Assets.Scripts.Networking;
using Newtonsoft.Json;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class PostServer : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "server" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            ServerPayload payload = null;
            try
            {
                payload = JsonConvert.DeserializeObject<ServerPayload>(e.Body);
            }
            catch
            {
                e.Context.SendResponse(500, new ErrorPayload()
                {
                    message = "Expected body to be ServerPayload."
                });
                return;
            }

            var serverInstance = SteamServer.Instance;
            if (payload.name != null && payload.name.Length > 0)
            {
                serverInstance.ServerName.text = payload.name;
                var prop = serverInstance.GetType().GetField("ServerNameText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop.SetValue(serverInstance, payload.name);
            }

            if (payload.password != null)
            {
                serverInstance.ServerName.text = payload.password;
                var prop = serverInstance.GetType().GetField("ServerNameText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop.SetValue(serverInstance, payload.password);
            }

            var response = ServerPayload.FromSteamServer(SteamServer.Instance);
            e.Context.SendResponse(200, response);
        }
    }
}