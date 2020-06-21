
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;
using WebAPI.Server.Attributes;

namespace WebAPI.Controllers
{
    [WebController(Path = "api/login")]
    class LoginController
    {
        [WebRouteMethod(Method = "GET")]
        [WebRouteMethod(Method = "POST")]
        public async Task GetLogin(IHttpContext context)
        {
            var user = await Authenticator.Authenticate(context);
            if (user == null)
            {
                // Authenticate is responsible for sending the response.
                return;
            }

            var token = Authenticator.GenerateToken(user);
            context.Response.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            await context.SendResponse(HttpStatusCode.OK, LoginPayload.FromToken(token));
        }
    }
}