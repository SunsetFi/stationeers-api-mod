
using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;
using Assets.Scripts.Serialization;
using System.Linq;

namespace StationeersWebApi.Models
{
    public static class SettingsModel
    {
        public static string IP
        {
            get
            {
                return NetworkManager.GetPublicIpAddress();
            }
        }

        public static string Name
        {
            get
            {
                return Settings.CurrentData.ServerName;
            }
            set
            {
                Settings.CurrentData.ServerName = value;
                NetworkManager.UpdateSessionData(Settings.CurrentData);
            }
        }

        public static string Password
        {
            get
            {
                return Settings.CurrentData.ServerPassword;
            }
            set
            {
                Settings.CurrentData.ServerPassword = value;
                NetworkManager.UpdateSessionData(Settings.CurrentData);
            }
        }

        public static int MaxPlayers
        {
            get
            {
                return Settings.CurrentData.ServerMaxPlayers;
            }
            set
            {
                Settings.CurrentData.ServerMaxPlayers = value;
                NetworkManager.UpdateSessionData(Settings.CurrentData);
            }
        }

        public static string MapName
        {
            get
            {
                return WorldManager.CurrentWorldName;
            }
        }

        public static GameState GameStatus
        {
            get
            {
                return GameManager.GameState;
            }
        }
    }
}