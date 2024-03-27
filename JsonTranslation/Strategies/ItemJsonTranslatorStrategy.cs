

using Assets.Scripts.Objects;

namespace StationeersWebApi.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(Item))]
    public sealed class ItemJsonTranslatorStrategy
    {
        [JsonPropertyGetter("parentSlotReferenceId")]
        public string GetParentSlotReferenceId(Item item)
        {
            var slot = item.ParentSlot;
            if (slot == null)
            {
                return null;
            }
            return slot.Parent.ReferenceId.ToString();
        }

        [JsonPropertyGetter("parentSlotId")]
        public int? GetParentSlotId(Item item)
        {
            var slot = item.ParentSlot;
            if (slot == null)
            {
                return null;
            }
            return slot.SlotId;
        }

        [JsonPropertyGetter("quantityText")]
        public string GetQuantityText(Item item)
        {
            return item.GetQuantityText();
        }
    }
}