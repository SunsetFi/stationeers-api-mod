using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Chat
{
    class GetChat : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "chat" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            var text = await Dispatcher.RunOnMainThread(() =>
            {
                // Chat panel just contains text.  If we want the real display name, we should record the chat network messages.
                return ChatPanel.Instance.Text.text.Split('\n').Select(message =>
                {
                    var parts = message.Split(':');
                    return new ChatPayload()
                    {
                        displayName = parts[0],
                        message = string.Join(":", parts.Skip(1))
                    };
                }).ToArray();
            });
            await context.SendResponse(HttpStatusCode.OK, text);
        }
    }
}