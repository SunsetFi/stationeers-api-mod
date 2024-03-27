
#if TODO_BANNED_PLAYERS

using System.Collections.Generic;
using Assets.Scripts;
using StationeersWebApi.Payloads;

namespace StationeersWebApi.Models
{

    public static class BansModel
    {
        private static List<string> Bans
        {
            get
            {
                return Reflection.GetPrivateField<Dictionary<ulong, string>>(BlockedPlayerManager.Instance, "Blocked");
            }
        }

        public static IList<BanPayload> GetBans()
        {
            var banPayloads = new List<BanPayload>();
            foreach (var item in BansModel.Bans)
            {
                banPayloads.Add(BanPayload.FromBanManagerData(item.Key, item.Value));
            }
            return banPayloads;
        }

        public static BanPayload GetBan(ulong steamId)
        {
            string remaining;
            if (BansModel.Bans.TryGetValue(steamId, out remaining))
            {
                return BanPayload.FromBanManagerData(steamId, remaining);
            }
            return null;
        }

        public static void AddBan(ulong steamId, double hours, string reason)
        {
            BlockedPlayerManager.Instance.SetBanPlayer(steamId, hours, reason.Length > 0 ? reason : "");
        }

        public static bool RemoveBan(ulong steamId)
        {
            return BlockedPlayerManager.Instance.RemoveBanPlayer(steamId);
        }
    }
}
#endif