
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;

namespace WebAPI.Routes.Server
{
    class PostServer : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "server" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

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
                if (payload.name != null && payload.name.Length != 0)
                {
                    ServerModel.Name = payload.name;
                }

                if (payload.password != null)
                {
                    ServerModel.Password = payload.password;
                }

                return ServerPayload.FromSteamServer(SteamServer.Instance);
            });

            await context.SendResponse(HttpStatusCode.OK, response);
        }
    }
}