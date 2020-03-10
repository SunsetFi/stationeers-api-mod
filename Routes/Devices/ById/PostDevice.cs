
using System.Collections.Generic;
using Assets.Scripts.Objects.Pipes;
using Newtonsoft.Json;
using WebAPI.API;

namespace WebAPI.Routes.Devices.ById
{
    class PostDevice : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "devices", ":deviceId" };

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

            DeviceItem item = null;
            try
            {
                item = JsonConvert.DeserializeObject<DeviceItem>(e.Body);
            }
            catch
            {
                e.Context.SendResponse(500, new Error()
                {
                    message = "Expected body to be DeviceItem."
                });
                return;
            }

            if (item.customName != null && item.customName.Length > 0)
            {
                device.CustomName = item.customName;
                device.IsCustomName = true;
            }

            e.Context.SendResponse(200, DeviceItem.FromDevice(device));
        }
    }
}