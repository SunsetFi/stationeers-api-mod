
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ceen;
using JWT.Algorithms;
using JWT.Builder;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes
{
    class PostLogin : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "login" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            if (context.Request.QueryString.Count == 0)
            {
                await context.SendResponse(HttpStatusCode.BadRequest, new ErrorPayload()
                {
                    message = "Login must forward a steam openid authentication response."
                });
                return;
            }

            try
            {
                var user = Authenticator.VerifyLogin(context.Request.QueryString);

                var token = Authenticator.GenerateToken(user);

                // This is the desired way of doing things, but having trouble reading it from the client.
                //  CORS, probably.
                context.Response.Headers.Add("Authorization", string.Format("Bearer {0}", token));

                await context.SendResponse(HttpStatusCode.OK, LoginPayload.FromApiUser(user, token));
                return;
            }
            catch (AuthenticationException)
            {
                await context.SendResponse(HttpStatusCode.Unauthorized, new ErrorPayload()
                {
                    message = "Unauthorized."
                });
                return;
            }
        }
    }
}