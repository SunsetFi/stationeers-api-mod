
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class StationContactsModel
    {
        public static IList<StationContactPayload> GetStationContacts()
        {
            return StationContact.AllStationContacts.Select(contact => StationContactPayload.FromStationContact(contact)).ToList();
        }

        public static StationContactPayload GetStationContact(int contactId)
        {
            var contact = StationContact.AllStationContacts.FirstOrDefault(x => x.ContactID == contactId);
            if (contact == null)
            {
                return null;
            }
            return StationContactPayload.FromStationContact(contact);
        }

        public static StationContactPayload UpdateStationContact(int contactId, StationContactPayload updates)
        {
            var contact = StationContact.AllStationContacts.FirstOrDefault(x => x.ContactID == contactId);
            if (contact == null)
            {
                return null;
            }

            StationContactsModel.WriteStationContactProperties(contact, updates);
            return StationContactPayload.FromStationContact(contact);
        }

        public static void WriteStationContactProperties(StationContact contact, StationContactPayload payload)
        {
            contact.Angle = Vector3Payload.ToVector3(payload.angle);
            contact.ContactName = payload.contactName;
            contact.ContactType = payload.contactType;
            contact.Lifetime = payload.lifetime;
            contact.EndLifetime = payload.endLifetime;
            contact.InitialLifeTime = payload.initialLifeTime;
            contact.ContactID = payload.contactID;
            // contact.HumanTradingSteamID = payload.HumanTradingSteamID;
            // contact.CurrentlyTrading = payload.currentlyTrading;
            // contact.ConnectedPad = payload.connectedPad;
            contact.TraderInventoryDict = payload.traderInventoryDict;
            contact.SerializedTraderInventory = payload.serializedTraderInventory;
        }

        public static StationContactPayload RemoveStationContact(int contactId)
        {
            var contact = StationContact.AllStationContacts.FirstOrDefault(x => x.ContactID == contactId);
            if (contact == null)
            {
                return null;
            }
            StationContact.AllStationContacts.Remove(contact);
            return StationContactPayload.FromStationContact(contact);    
        }
    }
}