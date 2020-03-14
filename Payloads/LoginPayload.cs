
using WebAPI.Authentication;

namespace WebAPI.Payloads
{
    public class LoginPayload
    {
        public string steamId { get; set; }

        public static LoginPayload FromApiUser(ApiUser user)
        {
            return new LoginPayload()
            {
                steamId = user.steamId.ToString()
            };
        }
    }
}