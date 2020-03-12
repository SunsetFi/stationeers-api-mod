
using System;
using System.IO;
using Newtonsoft.Json;

namespace WebAPI
{
    public class Config
    {
        public Config()
        {
            this.enabled = true;
        }

        public bool enabled { get; set; }
        public string password { get; set; }

        public static Config _instance;
        public static Config Instance
        {
            get
            {
                return Config._instance;
            }
        }

        public static void LoadConfig()
        {
            var assemblyLocation = typeof(Config).Assembly.Location;
            var assemblyDir = Path.GetDirectoryName(assemblyLocation);
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