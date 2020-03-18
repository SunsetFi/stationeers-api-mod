
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WebAPI
{
    class ConfigBody
    {
        public bool enabled { get; set; }
        public int port { get; set; }
        public string protocol { get; set; }
        public bool steamAuthentication { get; set; }
        public string[] allowedSteamIds { get; set; }
        public string plaintextPassword { get; set; }
        public string jwtSecret { get; set; }

        public ConfigBody()
        {
            this.enabled = true;
            this.port = 4444;
            this.protocol = "http";
            this.jwtSecret = Guid.NewGuid().ToString();
            this.steamAuthentication = false;
        }

        public void Validate()
        {
            if (this.port <= 0)
            {
                throw new Exception("Invalid Port.");
            }
            if (this.protocol != "http" && this.protocol != "https")
            {
                throw new Exception("Invalid Protocol.");
            }
        }
    }

    public static class Config
    {
        private static ConfigBody _instance;

        public static bool Enabled { get { return Config._instance.enabled; } }
        public static int Port { get { return Config._instance.port; } }
        public static string Protocol { get { return Config._instance.protocol; } }
        public static bool SteamAuthentication { get { return Config._instance.steamAuthentication; } }
        public static string[] AllowedSteamIds { get { return Config._instance.allowedSteamIds; } }
        public static string PlaintextPassword { get { return Config._instance.plaintextPassword; } }
        public static string JWTSecret { get { return Config._instance.jwtSecret; } }


        public static bool HasAuthentication
        {
            get
            {
                return Config._instance.steamAuthentication || !string.IsNullOrEmpty(Config._instance.plaintextPassword);
            }
        }



        public static void LoadConfig()
        {
            var assemblyDir = WebAPIPlugin.AssemblyDirectory;
            var path = Path.Combine(assemblyDir, "config.json");
            WebAPIPlugin.Instance.Log("Loading config at: " + path);

            string configText;
            try
            {
                configText = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                WebAPIPlugin.Instance.Log("No config file present.");
                _instance = new ConfigBody();
                return;
            }

            try
            {
                Config._instance = JsonConvert.DeserializeObject<ConfigBody>(configText);
                WebAPIPlugin.Instance.Log("Config loaded successfully.");
            }
            catch (Exception e)
            {
                Logging.Log(
                    new Dictionary<string, string>() {
                        {"ConfigPath", path}
                    },
                    "Failed to load config file: " + e.Message
                );
                _instance = new ConfigBody()
                {
                    enabled = false
                };
                return;
            }

            try
            {
                Config._instance.Validate();
            }
            catch (Exception e)
            {
                Logging.Log(
                new Dictionary<string, string>() {
                    {"ConfigPath", path}
                },
                "Invalid configuration: " + e.Message
                );
                Config._instance.enabled = false;
            }
        }
    }
}