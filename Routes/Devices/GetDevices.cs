
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class GetDevices : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            var payload = Dispatcher.RunOnMainThread(() => Device.AllDevices.Select(x => DevicePayload.FromDevice(x)));
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}