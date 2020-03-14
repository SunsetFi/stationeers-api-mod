
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Ceen;
using Steamworks;
using WebAPI.Payloads;

namespace WebAPI.Routes.Server
{
    class PostServer : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "server" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            ServerPayload payload = null;
            try
            {
                payload = context.ParseBody<ServerPayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be ServerPayload."
                });
                return;
            }

            var response = await Dispatcher.RunOnMainThread(() =>
            {
                var serverInstance = SteamServer.Instance;
                if (payload.name != null && payload.name.Length > 0)
                {
                    serverInstance.ServerName.text = payload.name;
                    var prop = serverInstance.GetType().GetField("ServerNameText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    prop.SetValue(serverInstance, payload.name);
                    SteamGameServer.SetServerName(payload.name);
                }

                if (payload.password != null)
                {
                    serverInstance.Password.text = payload.password;
                    var prop = serverInstance.GetType().GetField("PasswordText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    prop.SetValue(serverInstance, payload.password);
                    SteamGameServer.SetPasswordProtected(payload.password.Length > 0);
                }

                return ServerPayload.FromSteamServer(SteamServer.Instance);
            });

            await context.SendResponse(HttpStatusCode.OK, response);
        }
    }
}