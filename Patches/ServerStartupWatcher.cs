
using Assets.Scripts.Networking;
using HarmonyLib;

namespace WebAPI.Patches
{

    [HarmonyPatch(typeof(SteamServer), "StartSteamServer")]
    sealed class ServerStartupWatcher
    {
        static void Postfix()
        {
            WebAPIPlugin.Instance.StartServer();
        }
    }
}