
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices.ById.Logic
{
    class GetDeviceLogicValue : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices", ":deviceId", "logic", ":logicType" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
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

            var value = await Dispatcher.RunOnMainThread(() => device.GetLogicValue(type));

            await context.SendResponse(HttpStatusCode.OK, new LogicValuePayload() { value = value });
        }
    }
}