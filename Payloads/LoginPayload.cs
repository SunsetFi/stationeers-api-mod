

namespace StationeersWebApi.Payloads
{
    public class LoginPayload
    {
        public string authorization { get; set; }

        public static LoginPayload FromToken(string token)
        {
            return new LoginPayload()
            {
                authorization = token
            };
        }
    }
}