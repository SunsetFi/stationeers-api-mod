
using Assets.Scripts.Networking;
using HarmonyLib;

namespace StationeersWebApi.Patches
{

    [HarmonyPatch(typeof(SteamServer), "StartSteamServer")]
    sealed class ServerStartupWatcher
    {
        static void Postfix()
        {
            StationeersWebApiPlugin.Instance.StartServer();
        }
    }
}