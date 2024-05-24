
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace StationeersWebApi
{
    [JsonObject(MemberSerialization.OptIn)]
    class ConfigPasswordAuth
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public ConfigPasswordAuth()
        {
            this.Enabled = false;
        }

        public void Validate()
        {
            if (this.Enabled && string.IsNullOrEmpty(this.Password))
            {
                throw new Exception("Invalid Password.");
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    class ConfigSteamAuth
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("allowedSteamIds")]
        public string[] AllowedSteamIds { get; set; }

        public ConfigSteamAuth()
        {
            this.Enabled = false;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    class Config
    {
        public static Config Instance { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("passwordAuthentication")]
        public ConfigPasswordAuth PasswordAuthentication { get; set; } = new ConfigPasswordAuth();

        [JsonProperty("steamAuthentication")]
        public ConfigSteamAuth SteamAuthentication { get; set; } = new ConfigSteamAuth();

        [JsonProperty("jwtSecret")]
        public string JWTSecret { get; set; }

        public bool AuthenticationEnabled
        {
            get
            {
                return this.PasswordAuthentication.Enabled || this.SteamAuthentication.Enabled;
            }
        }

        public Config()
        {
            this.Enabled = true;
            this.Port = 8081;
        }

        public void Validate()
        {
            if (this.Port <= 0)
            {
                throw new Exception("Invalid Port.");
            }

            this.PasswordAuthentication.Validate();
        }

        public static void LoadConfig()
        {
            var assemblyDir = StationeersWebApiPlugin.AssemblyDirectory;
            var path = Path.Combine(assemblyDir, "config.json");
            Logging.LogTrace("Loading config at: " + path);

            string configText;
            try
            {
                configText = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                Logging.LogInfo("No config file present.");
                Instance = new Config();
                return;
            }

            try
            {
                Config.Instance = JsonConvert.DeserializeObject<Config>(configText);
                Logging.LogTrace("Config loaded successfully.");
            }
            catch (Exception e)
            {
                Logging.LogError(
                    new Dictionary<string, string>() {
                        {"ConfigPath", path}
                    },
                    "Failed to load config file: " + e.Message
                );
                Instance = new Config()
                {
                    Enabled = false
                };
                return;
            }

            try
            {
                Instance.Validate();
            }
            catch (Exception e)
            {
                Logging.LogError(
                new Dictionary<string, string>() {
                    {"ConfigPath", path}
                },
                "Invalid configuration: " + e.Message
                );
                Instance.Enabled = false;
            }
        }
    }
}