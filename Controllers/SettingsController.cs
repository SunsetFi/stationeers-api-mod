
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Models;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
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

                if (payload.startingCondition != null)
                {
                    SettingsModel.StartingCondition = payload.startingCondition;
                }

                if (payload.respawnCondition != null)
                {
                    SettingsModel.RespawnCondition = payload.respawnCondition;
                }

                return SettingsPayload.FromServer();
            });

            await context.SendResponse(HttpStatusCode.OK, response);
        }

        [WebRouteMethod(Method = "GET", Path = "respawn-conditions")]
        public async Task GetRespawnConditions(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => SettingsModel.AllRespawnConditions);
            await context.SendResponse(HttpStatusCode.OK, payload);
        }

        [WebRouteMethod(Method = "GET", Path = "starting-conditions")]
        public async Task GetStartingConditions(IHttpContext context)
        {
            Authenticator.VerifyAuth(context);

            var payload = await Dispatcher.RunOnMainThread(() => SettingsModel.AllStartingConditions);
            await context.SendResponse(HttpStatusCode.OK, payload);
        }
    }
}