
using Assets.Scripts.Networking;
using Assets.Scripts.Serialization;
using Steamworks;
using System;
using System.Linq;

namespace WebAPI.Models
{
    public static class SettingsModel
    {
        private static DateTime? lastSave;

        static SettingsModel()
        {
            XmlSaveLoad.OnSaveLoadFinished += OnSaveLoadFinished;
        }

        static void OnSaveLoadFinished()
        {
            SettingsModel.lastSave = DateTime.Now;
        }

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

        public static int MaxPlayers
        {
            get
            {
                return int.Parse(Reflection.GetPrivateField<string>(SteamServer.Instance, "MaxPlayerText"));
            }
            set
            {
                var serverInstance = SteamServer.Instance;
                if (serverInstance.MaxPlayer)
                {
                    serverInstance.MaxPlayer.text = value.ToString();
                }
                Reflection.SetPrivateField(serverInstance, "MaxPlayerText", value.ToString());
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

        public static DateTime? LastSave
        {
            get
            {
                return SettingsModel.lastSave;
            }
        }

        public static void ClearLastSave()
        {
            SettingsModel.lastSave = null;
        }
    }
}