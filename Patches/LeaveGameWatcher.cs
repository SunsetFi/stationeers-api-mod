
using Assets.Scripts;
using HarmonyLib;

namespace StationeersWebApi.Patches
{

    [HarmonyPatch(typeof(GameManager), "LeaveGame")]
    sealed class LeaveGameWatcher
    {
        static void Postfix()
        {
            StationeersWebApiPlugin.Instance.StopServer();
        }
    }
}