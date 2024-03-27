using System.Net;
using Assets.Scripts.AwayMission;
using HarmonyLib;

namespace StationeersWebApi.Patches
{

    [HarmonyPatch(typeof(SimpleHttpServerComponent), "StartServer")]
    sealed class SimpleHttpServerProcess
    {
        static bool Prefix()
        {
            // Suppress starting the rcon server so we can take over its port.
            return false;
        }
    }
}