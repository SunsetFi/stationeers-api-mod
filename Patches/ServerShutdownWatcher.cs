
using Assets.Scripts.Networking;
using HarmonyLib;

namespace StationeersWebApi.Patches
{

    [HarmonyPatch(typeof(SteamServer), "ShutdownGameServer")]
    sealed class ServerShutdownWatcher
    {
        static void Postfix()
        {
            StationeersWebApiPlugin.Instance.StopServer();
        }
    }
}