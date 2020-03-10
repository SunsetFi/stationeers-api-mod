

using Assets.Scripts.Objects;
using UnityEngine;

namespace WebAPI.API
{
    public class ThingItem
    {
        public long referenceId { get; set; }

        // For javascript usage: js does not support long values.
        public string referenceIdStr { get; set; }

        public string customName { get; set; }

        public Vector3Item position { get; set; }

        public static void CopyFromThing(ThingItem item, Thing thing)
        {
            item.referenceId = thing.ReferenceId;
            item.referenceIdStr = thing.ReferenceId.ToString();
            item.position = Vector3Item.FromVector3(thing.Position);
            item.customName = thing.IsCustomName ? thing.CustomName : null;
        }
    }
}