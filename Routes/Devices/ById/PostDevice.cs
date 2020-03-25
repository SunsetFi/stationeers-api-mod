
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices.ById
{
    // TODO: Move the Thing specific stuff from here to a shared class so non-Devices can use it too.
    class PostDevice : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "devices", ":deviceId" };

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

            DevicePayload payload = null;
            try
            {
                payload = context.ParseBody<DevicePayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be DevicePayload."
                });
                return;
            }

            await Dispatcher.RunOnMainThread(() =>
            {
                ThingModel.HandlePost(device, payload);
            });

            await context.SendResponse(HttpStatusCode.OK, DevicePayload.FromDevice(device));
        }
    }
}