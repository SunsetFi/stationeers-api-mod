
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WebAPI
{
    class ConfigBody
    {
        public bool enabled { get; set; }
        public int? port { get; set; }
        public string protocol { get; set; }
        public string authenticationMode { get; set; }
        public string[] allowedSteamIds { get; set; }
        public string plaintextPassword { get; set; }
        public string jwtSecret { get; set; }

        public ConfigBody()
        {
            this.enabled = true;
            this.port = null;
            this.protocol = "http";
            this.jwtSecret = Guid.NewGuid().ToString();
            this.authenticationMode = AuthenticationMode.None;
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
            if (!AuthenticationMode.isValid(this.authenticationMode))
            {
                throw new Exception("Invalid authentication mode.");
            }
        }
    }

    public static class AuthenticationMode
    {
        public const string None = "none";
        public const string Steam = "steam";

        public static bool isValid(string mode)
        {
            switch (mode)
            {
                case AuthenticationMode.None:
                case AuthenticationMode.Steam:
                    return true;
                default:
                    return false;
            }
        }
    }

    public static class Config
    {
        private static ConfigBody _instance;

        public static bool Enabled { get { return Config._instance.enabled; } }
        public static int? Port { get { return Config._instance.port; } }
        public static string Protocol { get { return Config._instance.protocol; } }
        public static string AuthenticationMode { get { return Config._instance.authenticationMode; } }
        public static string[] AllowedSteamIds { get { return Config._instance.allowedSteamIds; } }
        public static string PlaintextPassword { get { return Config._instance.plaintextPassword; } }
        public static string JWTSecret { get { return Config._instance.jwtSecret; } }

        public static void LoadConfig()
        {
            var assemblyDir = WebAPIPlugin.AssemblyDirectory;
            var path = Path.Combine(assemblyDir, "config.json");
            Logging.Log("Loading config at: " + path);

            string configText;
            try
            {
                configText = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                Logging.Log("No config file present.");
                _instance = new ConfigBody();
                return;
            }

            try
            {
                Config._instance = JsonConvert.DeserializeObject<ConfigBody>(configText);
                Logging.Log("Config loaded successfully.");
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