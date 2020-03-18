using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebAPI
{
    public static class Logging
    {
        public static string LogFilePath
        {
            get
            {
                var assemblyDir = WebAPIPlugin.AssemblyDirectory;
                var path = Path.Combine(assemblyDir, "log.txt");
                return path;
            }
        }

        public static void Log(string message, params object[] args)
        {
            Logging.Log(new Dictionary<string, string>(), message, args);
        }

        public static void Log(IDictionary<string, string> values, string message, params object[] args)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("DateTime={1} ", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            foreach (var key in values.Keys)
            {
                sb.AppendFormat("{0}={1} ", key, values[key]);
            }
            sb.Append("\n");
            sb.AppendFormat(message, args);

            UnityEngine.Debug.Log("[WebAPI]: " + sb.ToString().Replace("\n", "\n\t"));

            sb.Append("\n\n");
            File.AppendAllText(Logging.LogFilePath, sb.ToString());
        }
    }
}