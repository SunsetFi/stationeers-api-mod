
using System.Threading.Tasks;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using StationeersWebApi.Authentication;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;
using StationeersWebApi.Server.Exceptions;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/devices/:referenceId/logic")]
    public class DeviceLogicController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetAllLogicValues(IHttpContext context, long referenceId)
        {
            Authenticator.VerifyAuth(context);

            var device = await Dispatcher.RunOnMainThread(() => Device.AllDevices.Find(x => x.ReferenceId == referenceId));
            if (device == null)
            {
                throw new NotFoundException("Device not found.");
            }

            var payload = Dispatcher.RunOnMainThread(() => LogicableItemUtils.GetLogicValues(device));

            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "GET", Path = ":logicType")]
        // FIXME: Make logicType parameter case insensitive
        public async Task GetLogicValue(IHttpContext context, long referenceId, LogicType logicType)
        {
            Authenticator.VerifyAuth(context);

            var device = await Dispatcher.RunOnMainThread(() => Device.AllDevices.Find(x => x.ReferenceId == referenceId));
            if (device == null)
            {
                throw new NotFoundException("Device not found.");
            }

            var payload = await Dispatcher.RunOnMainThread(() => LogicableItemUtils.GetLogicValue(device, logicType));

            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "POST", Path = ":logicType")]
        public async Task SetLogicValue(IHttpContext context, long referenceId, LogicType logicType, LogicValuePayload body)
        {
            Authenticator.VerifyAuth(context);

            var device = await Dispatcher.RunOnMainThread(() => Device.AllDevices.Find(x => x.ReferenceId == referenceId));
            if (device == null)
            {
                throw new NotFoundException("Device not found.");
            }

            var canLogicWrite = await Dispatcher.RunOnMainThread(() => device.CanLogicWrite(logicType));
            if (!canLogicWrite)
            {
                throw new BadRequestException("Logic type is not writable.");
            }

            await Dispatcher.RunOnMainThread(() => device.SetLogicValue(logicType, body.value));

            await context.SendResponse(HttpStatusCode.OK, body);
        }
    }
}