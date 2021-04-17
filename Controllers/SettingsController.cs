
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

            var response = await Dispatcher.RunOnMainThread(() =>
            {
                if (body.name != null && body.name.Length != 0)
                {
                    SettingsModel.Name = body.name;
                }

                if (body.maxPlayers.HasValue)
                {
                    SettingsModel.MaxPlayers = body.maxPlayers.Value;
                }

                if (body.password != null)
                {
                    SettingsModel.Password = body.password;
                }

                if (body.startingCondition != null)
                {
                    SettingsModel.StartingCondition = body.startingCondition;
                }

                if (body.respawnCondition != null)
                {
                    SettingsModel.RespawnCondition = body.respawnCondition;
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