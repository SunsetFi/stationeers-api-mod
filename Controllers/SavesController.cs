
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Scripts.Serialization;
using Assets.Scripts.Util;
using Ceen;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

namespace WebAPI.Controllers
{
    [WebController(Path = "api/saves")]
    public class SavesController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetSave(IHttpContext context)
        {
            await context.SendResponse(HttpStatusCode.OK, SavePayload.FromServer());
        }

        [WebRouteMethod(Method = "POST")]
        public async Task PostSave(IHttpContext context, PostSavePayload body)
        {
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