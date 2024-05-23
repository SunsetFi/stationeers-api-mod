
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Models;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/settings")]
    public class SettingsController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task GetSettings(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => SettingsPayload.FromServer());
            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "POST")]
        public async Task PostSettings(IHttpContext context, SettingsPayload body)
        {
            Authenticator.VerifyAuth(context);

            SettingsPayload payload = null;
            try
            {
                payload = context.ParseBody<SettingsPayload>();
            }
            catch
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Expected body to be ServerPayload."
                });
                return;
            }

            var response = await Dispatcher.RunOnMainThread(() =>
            {
                if (payload.name != null && payload.name.Length != 0)
                {
                    SettingsModel.Name = payload.name;
                }

                if (payload.maxPlayers.HasValue)
                {
                    SettingsModel.MaxPlayers = payload.maxPlayers.Value;
                }

                if (payload.password != null)
                {
                    SettingsModel.Password = payload.password;
                }

                return SettingsPayload.FromServer();
            });

            await context.SendResponse(HttpStatusCode.OK, response);
        }
    }
}