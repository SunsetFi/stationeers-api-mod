
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StationeersWebApi;
using StationeersWebApi.Server;
using StationeersWebApi.Server.Exceptions;

namespace WebAPI.Authentication.Strategies.Password
{
    public class PasswordAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<ApiUser> TryAuthenticate(IHttpContext context)
        {
            if (!Config.Instance.PasswordAuthentication.Enabled)
            {
                throw new NotFoundException();
            }

            var password = context.QueryString["password"];
            if (string.IsNullOrEmpty(password))
            {
                throw new BadRequestException("Password query parameter is required.");
            }

            if (password != Config.Instance.PasswordAuthentication.Password)
            {
                throw new UnauthorizedException();
            }

            await context.SendResponse(HttpStatusCode.OK);
            return PasswordApiUser.MakePasswordUser(context, password);
        }

        public void Verify(IHttpContext context, JObject authToken, out ApiUser user)
        {
            PasswordApiUser passwordUser = authToken.ToObject<PasswordApiUser>();
            if (passwordUser == null)
            {
                throw new UnauthorizedException();
            }

            user = passwordUser;

            ApiUser.VerifyUser(passwordUser, context);

            // TODO: Verify password has not changed.
            //  Disabling this for now.  GetHashCode is returning inconsistent results between checks.
            // if (passwordUser.PasswordHash != Config.Instance.PasswordAuthentication.Password.GetHashCode())
            // {
            //     Logging.Log("Password verify with wrong password");
            //     throw new UnauthorizedException();
            // }
        }
    }
}