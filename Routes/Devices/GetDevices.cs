
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class GetDevices : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() =>
            {
                // Devices can have duplicates in this list.
                var set = new HashSet<Device>();
                foreach (var device in Device.AllDevices)
                {
                    set.Add(device);
                }
                return set.Select(x => DevicePayload.FromDevice(x));
            });
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}