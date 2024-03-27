
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Scripts.Serialization;
using Assets.Scripts.Util;
using Ceen;
using StationeersWebApi.Authentication;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/saves")]
    public class SavesController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetSave(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);
            await context.SendResponse(HttpStatusCode.OK, SavePayload.FromServer());
        }

        [WebRouteMethod(Method = "POST")]
        public async Task PostSave(IHttpContext context, PostSavePayload body)
        {
            Authenticator.VerifyAuth(context);

            if (body == null)
            {
                throw new BadRequestException("A body with a fileName must be specified.");
            }

            if (body.fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new BadRequestException("Filename contains invalid characters.");
            }

            string worldDirectory = Path.Combine(XmlSaveLoad.CheckFiles(), "saves/" + body.fileName);
            UnityMainThreadDispatcher.Instance().Enqueue(XmlSaveLoad.Instance.ForceWriteWorld(worldDirectory));
            await context.SendResponse(HttpStatusCode.OK, body);
        }
    }
}