
using Assets.Scripts.Networking;
using Steamworks;

namespace WebAPI.Models
{
    public static class ServerModel
    {
        public static string IP
        {
            get
            {
                return NetworkManagerHudOverride.Instance.Manager.serverBindAddress;
            }
        }

        public static string Name
        {
            get
            {
                return Reflection.GetPrivateField<string>(SteamServer.Instance, "ServerNameText");
            }
            set
            {
                var serverInstance = SteamServer.Instance;
                if (serverInstance.ServerName)
                {
                    serverInstance.ServerName.text = value;
                }
                Reflection.SetPrivateField(serverInstance, "ServerNameText", value);
                SteamGameServer.SetServerName(value);
            }
        }

        public static string Password
        {
            get
            {
                return Reflection.GetPrivateField<string>(SteamServer.Instance, "PasswordText");
            }
            set
            {
                var serverInstance = SteamServer.Instance;
                if (serverInstance.Password)
                {
                    serverInstance.Password.text = value;
                }
                Reflection.SetPrivateField(serverInstance, "PasswordText", value);
                SteamGameServer.SetPasswordProtected(value.Length > 0);
            }
        }
    }
}