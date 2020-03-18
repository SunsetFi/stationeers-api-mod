
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Server
{
    class PostMessage : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "server", "message" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            ServerMessagePayload payload = null;
            try
            {
                payload = context.ParseBody<ServerMessagePayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be ServerMessagePayload."
                });
                return;
            }

            if (payload.message == null || payload.message.Length == 0)
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected a message."
                });
                return;
            }

            await Dispatcher.RunOnMainThread(() =>
            {
                NetworkManagerHudOverride.Instance.SendNoticeMessage(payload.message);
            });

            await context.SendResponse(HttpStatusCode.OK, null);
        }
    }
}