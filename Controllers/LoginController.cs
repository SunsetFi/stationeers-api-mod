
using System.Collections.Generic;
using System.Threading.Tasks;
using StationeersWebApi.Authentication;
using StationeersWebApi.Payloads;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Attributes;

namespace StationeersWebApi.Controllers
{
    [WebController(Path = "api/login")]
    class LoginController
    {
        [WebRouteMethod(Method = "GET")]
        public async Task Login(IHttpContext context)
        {
            var loginMethods = new List<string>();
            if (Config.Instance.PasswordAuthentication.Enabled)
            {
                loginMethods.Add("password");
            }
            if (Config.Instance.SteamAuthentication.Enabled)
            {
                loginMethods.Add("steam");
            }

            var response = new LoginMethodsPayload
            {
                loginMethods = loginMethods
            };

            await context.SendResponse(HttpStatusCode.OK, response);
        }

        [WebRouteMethod(Path = "steam", Method = "GET")]
        public async Task SteamLogin(IHttpContext context)
        {
            var user = await Authenticator.Authenticate(context, AuthenticationMethod.Steam);
            if (user == null)
            {
                // Authenticate is responsible for sending the response.
                return;
            }

            var token = Authenticator.GenerateToken(user);
            context.SetResponseHeader("Authorization", string.Format("Bearer {0}", token));
            await context.SendResponse(HttpStatusCode.OK, LoginPayload.FromToken(token));
        }

        [WebRouteMethod(Path = "password", Method = "POST")]
        public async Task PasswordLogin(IHttpContext context)
        {
            var user = await Authenticator.Authenticate(context, AuthenticationMethod.Password);
            if (user == null)
            {
                // Authenticate is responsible for sending the response.
                return;
            }

            var token = Authenticator.GenerateToken(user);
            context.SetResponseHeader("Authorization", string.Format("Bearer {0}", token));
            await context.SendResponse(HttpStatusCode.OK, LoginPayload.FromToken(token));
        }
    }
}