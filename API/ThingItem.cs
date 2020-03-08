

using Assets.Scripts.Objects;
using UnityEngine;

namespace WebAPI.API
{
    public class ThingItem
    {
        // FIXME: json does not support longs.
        public long referenceId { get; set; }

        public Vector3Item position { get; set; }

        public static void CopyFromThing(ThingItem item, Thing thing)
        {
            item.referenceId = thing.ReferenceId;
            item.position = Vector3Item.FromVector3(thing.Position);
        }
    }
}