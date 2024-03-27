
namespace StationeersWebApi.Authentication.Strategies.Anonymous
{
    public class AnonymousApiUser : ApiUser
    {
        public AnonymousApiUser()
        : base(Authentication.AuthenticationMethod.Anonymous)
        {
        }
    }
}