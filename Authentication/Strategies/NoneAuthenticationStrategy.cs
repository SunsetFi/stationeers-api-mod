
using System.Threading.Tasks;
using Ceen;

namespace WebAPI.Authentication.Strategies
{
    public class NoneAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<AuthenticationResult> TryAuthenticate(IHttpContext context)
        {
            await context.SendResponse(HttpStatusCode.OK);
            return new AuthenticationResult(true, ApiUser.MakeRootUser(context));
        }

        public bool TryVerify(IHttpContext context, out ApiUser user)
        {
            user = ApiUser.MakeRootUser(context);
            return true;
        }
    }
}