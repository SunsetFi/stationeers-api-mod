
using System.Collections.Generic;
using Assets.Scripts.Objects.Pipes;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices.ById.Logic
{
    class GetDeviceLogic : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices", ":deviceId", "logic" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            // TODO: Return UNPROCESSABLE_ENTITY if deviceId invalid.
            var referenceId = long.Parse(pathParams["deviceId"]);
            var device = Device.AllDevices.Find(x => x.ReferenceId == referenceId);
            if (device == null)
            {
                e.Context.SendResponse(404, new ErrorPayload()
                {
                    message = "Device not found."
                });
                return;
            }

            e.Context.SendResponse(200, LogicableItemUtils.GetLogicValues(device));
        }
    }
}