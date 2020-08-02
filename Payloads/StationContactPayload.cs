
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;

namespace WebAPI.Payloads
{
    public class StationContactPayload
    {
		public Vector3Payload angle { get; set; }
		public string contactName { get; set; }
		public StationContactType contactType { get; set; }
		public float lifetime { get; set; }
		public float endLifetime { get; set; }
		public float initialLifeTime { get; set; }
		public int contactID { get; set; }
		public ulong humanTradingSteamID { get; set; }
		public bool currentlyTrading { get; set; }
		public LandingPad connectedPad { get; set; }
		public Dictionary<int, TradingItemDat> traderInventoryDict { get; set; }
		public List<TradingItemDat> serializedTraderInventory { get; set; }

        public static StationContactPayload FromStationContact(StationContact contact)
        {
            var payload = new StationContactPayload
            {
                angle = Vector3Payload.FromVector3(contact.Angle),
                contactName = contact.ContactName,
                contactType = contact.ContactType,
                lifetime = contact.Lifetime,
                endLifetime = contact.EndLifetime,
                initialLifeTime = contact.InitialLifeTime,
                contactID = contact.ContactID,
                humanTradingSteamID = contact.HumanTradingSteamID,
                currentlyTrading = contact.CurrentlyTrading,
                connectedPad = contact.ConnectedPad,
                traderInventoryDict = contact.TraderInventoryDict,
                serializedTraderInventory = contact.SerializedTraderInventory,
            };
            return payload;
        }
    }
}