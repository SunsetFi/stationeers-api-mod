
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using WebAPI.Server;

namespace WebAPI.Routes.Devices.ById.Logic
{
    class GetDeviceLogicValue : IWebRoute
    {
        public string Method => "GET";

        public string Path => "devices/:deviceId/logic/:logicType";

        public async Task OnRequested(IWebRouteContext context)
        {
            Authenticator.VerifyAuth(context);

            var pathParams = context.PathParameters;

            // TODO: Return UNPROCESSABLE_ENTITY if deviceId invalid.
            var referenceId = long.Parse(pathParams["deviceId"]);
            var device = await Dispatcher.RunOnMainThread(() => Device.AllDevices.Find(x => x.ReferenceId == referenceId));
            if (device == null)
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Device not found."
                });
                return;
            }

            var typeName = pathParams["logicType"];
            LogicType type;
            if (!Enum.TryParse<LogicType>(typeName, out type))
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Unrecognized logic type."
                });
                return;
            }

            var payload = await Dispatcher.RunOnMainThread(() => LogicableItemUtils.GetLogicValue(device, type));

            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}