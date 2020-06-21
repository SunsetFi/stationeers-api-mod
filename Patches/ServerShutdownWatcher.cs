
using Assets.Scripts.Networking;
using HarmonyLib;

namespace WebAPI.Patches
{

    [HarmonyPatch(typeof(SteamServer), "ShutdownGameServer")]
    sealed class ServerShutdownWatcher
    {
        static void Postfix()
        {
            WebAPIPlugin.Instance.StopServer();
        }
    }
}