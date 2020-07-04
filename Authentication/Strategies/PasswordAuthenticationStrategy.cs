
using System.Threading.Tasks;
using Ceen;
using WebAPI.Server.Exceptions;

namespace WebAPI.Authentication.Strategies
{
    public class PasswordAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<ApiUser> TryAuthenticate(IHttpContext context)
        {
            var password = context.Request.QueryString["password"];
            if (string.IsNullOrEmpty(password))
            {
                throw new BadRequestException("Password query parameter is required.");
            }

            if (password != Config.PlaintextPassword)
            {
                throw new UnauthorizedException();
            }

            await context.SendResponse(HttpStatusCode.OK);
            return ApiUser.MakeRootUser(context);
        }

        public void Verify(IHttpContext context, out ApiUser user)
        {
            user = Authenticator.GetUserFromToken(context);
            if (user == null)
            {
                throw new UnauthorizedException();
            }
        }
    }
}