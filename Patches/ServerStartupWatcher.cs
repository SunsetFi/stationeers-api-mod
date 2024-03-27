
using Assets.Scripts.Networking;
using HarmonyLib;

namespace StationeersWebApi.Patches
{

    [HarmonyPatch(typeof(NetworkManager), "StartServer")]
    sealed class ServerStartupWatcher
    {
        static void Postfix()
        {
            StationeersWebApiPlugin.Instance.StartServer();
        }
    }
}