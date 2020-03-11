
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects.Pipes;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class GetDevices : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            var devices = Device.AllDevices.Select(x => DevicePayload.FromDevice(x));
            e.Context.SendResponse(200, devices);
        }
    }
}