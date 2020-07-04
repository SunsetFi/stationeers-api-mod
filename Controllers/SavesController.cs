
using System.IO;
using System.Threading.Tasks;
using Assets.Scripts.Serialization;
using Ceen;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;
using WebAPI.Server.Exceptions;

namespace WebAPI.Controllers
{
    [WebController(Path = "api/saves")]
    public class SavesController
    {
        [WebRouteMethod(Method = "POST")]
        public async Task PostSave(IHttpContext context, PostSavePayload body)
        {
            if (body.fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new BadRequestException("Filename contains invalid characters.");
            }

            string worldDirectory = Path.Combine(XmlSaveLoad.CheckFiles(), "saves/" + body.fileName);
            await Dispatcher.RunOnMainThread(() => XmlSaveLoad.Instance.ForceWriteWorld(worldDirectory));
            await context.SendResponse(HttpStatusCode.OK, body);
        }
    }
}