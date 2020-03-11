
using System;
using System.Collections.Generic;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices.ById.Logic
{
    class GetDeviceLogicValue : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices", ":deviceId", "logic", ":logicType" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            // TODO: Return UNPROCESSABLE_ENTITY if deviceId invalid.
            var referenceId = long.Parse(pathParams["deviceId"]);
            var device = Device.AllDevices.Find(x => x.ReferenceId == referenceId);
            if (device == null)
            {
                e.Context.SendResponse(404, new Error()
                {
                    message = "Device not found."
                });
                return;
            }

            var typeName = pathParams["logicType"];
            LogicType type;
            if (!Enum.TryParse<LogicType>(typeName, out type))
            {
                e.Context.SendResponse(404, new Error()
                {
                    message = "Unrecognized logic type."
                });
                return;
            }

            var value = device.GetLogicValue(type);

            e.Context.SendResponse(200, new LogicValueItem() { value = value });
        }
    }
}