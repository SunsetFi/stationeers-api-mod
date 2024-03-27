using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StationeersWebApi
{
    public static class Logging
    {
        public static string LogFilePath
        {
            get
            {
                var assemblyDir = StationeersWebApiPlugin.AssemblyDirectory;
                var path = Path.Combine(assemblyDir, "log.txt");
                return path;
            }
        }

        public static void Log(string message, params object[] args)
        {
            Logging.Log(new Dictionary<string, string>(), message, args);
        }

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The log message format string.</param>
        /// <param name="args">An object array that contains zero or more items to format.</param>
        public static void LogTrace(string message, params object[] args)
        {
            Log(new Dictionary<string, string>(), message, args);
        }

        /// <summary>
        /// Logs a trace message with associated key-value pairs.
        /// </summary>
        /// <param name="values">A dictionary containing key-value pairs to be included in the log entry.</param>
        /// <param name="message">The log message format string.</param>
        /// <param name="args">An object array that contains zero or more items to format.</param>
        public static void LogTrace(IDictionary<string, string> values, string message, params object[] args)
        {
            Log(values, message, args);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The log message format string.</param>
        /// <param name="args">An object array that contains zero or more items to format.</param>
        public static void LogInfo(string message, params object[] args)
        {
            Log(new Dictionary<string, string>(), message, args);
        }

        /// <summary>
        /// Logs an informational message with associated key-value pairs.
        /// </summary>
        /// <param name="values">A dictionary containing key-value pairs to be included in the log entry.</param>
        /// <param name="message">The log message format string.</param>
        /// <param name="args">An object array that contains zero or more items to format.</param>
        public static void LogInfo(IDictionary<string, string> values, string message, params object[] args)
        {
            Log(values, message, args);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The log message format string.</param>
        /// <param name="args">An object array that contains zero or more items to format.</param>
        public static void LogError(string message, params object[] args)
        {
            Log(new Dictionary<string, string>(), message, args);
        }

        /// <summary>
        /// Logs an error message with associated key-value pairs.
        /// </summary>
        /// <param name="values">A dictionary containing key-value pairs to be included in the log entry.</param>
        /// <param name="message">The log message format string.</param>
        /// <param name="args">An object array that contains zero or more items to format.</param>
        public static void LogError(IDictionary<string, string> values, string message, params object[] args)
        {
            Log(values, message, args);
        }


        public static void Log(IDictionary<string, string> values, string message, params object[] args)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("DateTime={0} ", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            foreach (var key in values.Keys)
            {
                sb.AppendFormat("{0}={1} ", key, values[key]);
            }
            sb.Append("\n");
            sb.AppendFormat(message, args);

            UnityEngine.Debug.Log("[StationeersWebApi]: " + sb.ToString().Replace("\n", "\n\t"));

            sb.Append("\n\n");
            File.AppendAllText(Logging.LogFilePath, sb.ToString());
        }
    }
}