
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices.ById.Logic
{
    class PostDeviceLogicValue : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "devices", ":deviceId", "logic", ":logicType" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

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

            var canLogicWrite = await Dispatcher.RunOnMainThread(() => device.CanLogicWrite(type));
            if (!canLogicWrite)
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Logic type is not writable."
                });
                return;
            }

            LogicValuePayload payload = null;
            try
            {
                payload = context.ParseBody<LogicValuePayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be LogicValueItem."
                });
                return;
            }

            await Dispatcher.RunOnMainThread(() => device.SetLogicValue(type, payload.value));

            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}