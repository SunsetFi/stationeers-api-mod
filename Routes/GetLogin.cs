
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes
{
    // We need a GET version of login for openid support
    class GetLogin : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "login" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
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