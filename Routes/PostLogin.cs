
using System.Collections.Generic;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices
{
    class PostLogin : IWebRoute
    {
        public string Method => "POST";

        public string[] Segments => new[] { "login" };

        public void OnRequested(RequestEventArgs e, IDictionary<string, string> pathParams)
        {
            if (e.Context.Request.Url.Query.Length == 0)
            {
                e.Context.SendResponse(400, new ErrorPayload()
                {
                    message = "Login must forward a steam openid authentication response."
                });
                return;
            }

            try
            {
                var user = Authenticator.VerifyLogin(e.Context.Request.Url);
                e.Context.SendResponse(200, LoginPayload.FromApiUser(user));
                return;
            }
            catch (AuthenticationException)
            {
                e.Context.SendResponse(401, new ErrorPayload()
                {
                    message = "Unauthorized."
                });
                return;
            }
        }
    }
}