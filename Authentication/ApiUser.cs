
namespace WebAPI.Authentication
{
    public class ApiUser
    {
        public bool isSteamUser { get; set; }
        public string steamId { get; set; }

        public bool isRootUser { get; set; }
    }
}