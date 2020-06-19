using System.Net;
using Assets.Scripts.AwayMission;
using HarmonyLib;

namespace WebAPI
{

    [HarmonyPatch(typeof(SimpleHttpServerComponent), "StartServer")]
    class SimpleHttpServerProcess
    {
        static bool Prefix(HttpListenerContext context)
        {
            // Suppress starting the rcon server so we can take over its port.
            return false;
        }
    }
}