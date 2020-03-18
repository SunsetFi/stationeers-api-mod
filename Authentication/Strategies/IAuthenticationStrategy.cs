

using System.Threading.Tasks;
using Ceen;

namespace WebAPI.Authentication
{
    public interface IAuthenticationStrategy
    {
        Task<AuthenticationResult> TryAuthenticate(IHttpContext context);
        bool TryVerify(IHttpContext context, out ApiUser user);
    }

    public class AuthenticationResult
    {
        public bool Handled { get; private set; }
        public ApiUser User { get; private set; }
        public AuthenticationResult(bool handled, ApiUser user)
        {
            this.Handled = handled;
            this.User = user;
        }
    }
}