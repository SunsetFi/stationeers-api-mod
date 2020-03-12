
using System.Collections.Generic;
using Assets.Scripts.Networking;
using Newtonsoft.Json;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class PostMessage : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "server", "message" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            ServerMessagePayload payload = null;
            try
            {
                payload = JsonConvert.DeserializeObject<ServerMessagePayload>(e.Body);
            }
            catch
            {
                e.Context.SendResponse(400, new ErrorPayload()
                {
                    message = "Expected body to be ServerMessagePayload."
                });
                return;
            }

            if (payload.message == null || payload.message.Length == 0)
            {
                e.Context.SendResponse(400, new ErrorPayload()
                {
                    message = "Expected a message."
                });
            }

            NetworkManagerHudOverride.Instance.SendNoticeMessage(payload.message);

            e.Context.SendResponse(200, null);
        }
    }
}