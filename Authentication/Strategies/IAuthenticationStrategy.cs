

using System.Threading.Tasks;
using Ceen;

namespace WebAPI.Authentication
{
    public interface IAuthenticationStrategy
    {
        Task<ApiUser> TryAuthenticate(IHttpContext context);
        void Verify(IHttpContext context, out ApiUser user);
    }
}