
using Assets.Scripts.Networking;
using HarmonyLib;

namespace StationeersWebApi.Patches
{

    [HarmonyPatch(typeof(NetworkManager), "StopServer")]
    sealed class ServerShutdownWatcher
    {
        static void Postfix()
        {
            StationeersWebApiPlugin.Instance.StopServer();
        }
    }
}