

using Assets.Scripts.Objects;

namespace WebAPI.Payloads
{
    public class ThingItem
    {
        public string referenceId { get; set; }

        public string customName { get; set; }

        public Vector3Item position { get; set; }

        public static void CopyFromThing(ThingItem item, Thing thing)
        {
            item.referenceId = thing.ReferenceId.ToString();
            item.position = Vector3Item.FromVector3(thing.Position);
            item.customName = thing.IsCustomName ? thing.CustomName : null;
        }
    }
}