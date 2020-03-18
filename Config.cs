
using System;
using System.IO;
using Newtonsoft.Json;

namespace WebAPI
{
    public class Config
    {
        public static Config _instance;
        public static Config Instance
        {
            get
            {
                return Config._instance;
            }
        }

        public bool enabled { get; set; }
        public bool steamAuthentication { get; set; }
        public string[] allowedSteamIds { get; set; }
        public string plaintextPassword { get; set; }
        public int port { get; set; }
        public string jwtSecret { get; set; }

        public bool HasAuthentication
        {
            get
            {
                return this.steamAuthentication || !string.IsNullOrEmpty(this.plaintextPassword);
            }
        }

        public Config()
        {
            this.enabled = true;
            this.port = 4444;
            this.jwtSecret = Guid.NewGuid().ToString();
            this.steamAuthentication = false;
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
                _instance = new Config();
                return;
            }

            try
            {
                Config._instance = JsonConvert.DeserializeObject<Config>(configText);
                WebAPIPlugin.Instance.Log("Config loaded successfully.");
            }
            catch (Exception)
            {
                WebAPIPlugin.Instance.Log("Malformed config file.");
                _instance = new Config()
                {
                    enabled = false
                };
                return;
            }
        }
    }
}