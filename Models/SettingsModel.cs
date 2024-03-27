
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

        public static string StartingCondition
        {
            get
            {
                return WorldManager.CurrentStartCondition.Key;
            }
            set
            {
                // Validates the condition and does a no-op if invalid.
                WorldManager.SetStartCondition(value);
            }
        }

        public static string[] AllStartingConditions
        {
            get
            {
                return WorldManager.StartingConditions.Select(x => x.Key).ToArray();
            }
        }

        public static string RespawnCondition
        {
            get
            {
                return WorldManager.CurrentRespawnCondition.Key;
            }
            set
            {
                // Validates the condition and does a no-op if invalid.
                WorldManager.SetRespawnCondition(value);
            }
        }

        public static string[] AllRespawnConditions
        {
            get
            {
                return WorldManager.RespawnConditions.Select(x => x.Key).ToArray();
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