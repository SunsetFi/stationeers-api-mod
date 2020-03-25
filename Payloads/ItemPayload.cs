
using Assets.Scripts.Objects;

namespace WebAPI.Payloads
{
    // TODO: Split into different payloads based on item type.
    // We want to read/write things like battery charge, item quantity, and so on.
    public class ItemPayload : ThingPayload
    {
        public ItemPayload()
        {
            this.type = "item";
        }

        public string parentSlotReferenceId { get; set; }
        public int parentSlotId { get; set; }

        public string quantityText { get; set; }

        public static ItemPayload FromItem(Item item)
        {
            var payload = new ItemPayload();
            ThingPayload.CopyFromThing(payload, item);
            var slot = item.ParentSlot;
            if (slot != null)
            {
                payload.parentSlotReferenceId = slot.Parent.ReferenceId.ToString();
                payload.parentSlotId = slot.SlotId;
            }
            payload.quantityText = item.GetQuantityText();
            return payload;
        }
    }

}