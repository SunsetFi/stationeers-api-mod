
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
{

    [WebController(Path = "chat")]
    public class ChatController
    {
        public async Task GetChat(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var chat = Dispatcher.RunOnMainThread(() => ChatModel.GetChat());

            await context.SendResponse(HttpStatusCode.OK, chat);
        }
    }
}