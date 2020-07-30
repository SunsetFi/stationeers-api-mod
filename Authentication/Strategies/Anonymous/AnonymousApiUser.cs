
using JWT.Builder;

namespace WebAPI.Authentication.Strategies.Anonymous
{
    public class AnonymousApiUser : ApiUser
    {
        public override void SerializeToJwt(JwtBuilder builder)
        {
            base.SerializeToJwt(builder);
        }
    }
}