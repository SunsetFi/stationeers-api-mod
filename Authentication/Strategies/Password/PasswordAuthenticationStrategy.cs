
using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json.Linq;
using WebAPI.Server.Exceptions;

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

            var password = context.Request.QueryString["password"];
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
                throw new ForbiddenException();
            }

            user = passwordUser;

            ApiUser.VerifyUser(passwordUser, context);

            // We know they had the password at one point due to the valid jwt.
            //  However, the password may have changed since they last entered it.  Check for this.
            if (passwordUser.PasswordHash != Config.Instance.PasswordAuthentication.Password.GetHashCode())
            {
                throw new UnauthorizedException();
            }
        }
    }
}