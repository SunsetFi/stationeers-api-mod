
using WebAPI.Authentication;

namespace WebAPI.Payloads
{
    public class LoginPayload
    {
        public string steamId { get; set; }
        public string authorization { get; set; }

        public static LoginPayload FromApiUser(ApiUser user, string authorization)
        {
            return new LoginPayload()
            {
                steamId = user.steamId.ToString(),
                authorization = authorization
            };
        }
    }
}