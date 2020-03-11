

using Assets.Scripts.Objects;

namespace WebAPI.Payloads
{
    public class ThingPayload
    {
        public string referenceId { get; set; }

        public string customName { get; set; }

        public Vector3Payload position { get; set; }

        public static ThingPayload FromThing(Thing thing)
        {
            var payload = new ThingPayload();
            ThingPayload.CopyFromThing(payload, thing);
            return payload;
        }
        public static void CopyFromThing(ThingPayload payload, Thing thing)
        {
            payload.referenceId = thing.ReferenceId.ToString();
            payload.position = Vector3Payload.FromVector3(thing.Position);
            payload.customName = thing.IsCustomName ? thing.CustomName : null;
        }
    }
}