
using System.Threading.Tasks;
using Ceen;

namespace WebAPI.Authentication.Strategies
{
    public class NoneAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<ApiUser> TryAuthenticate(IHttpContext context)
        {
            await context.SendResponse(HttpStatusCode.OK);
            return ApiUser.MakeRootUser(context);
        }

        public void Verify(IHttpContext context, out ApiUser user)
        {
            user = ApiUser.MakeRootUser(context);
        }
    }
}