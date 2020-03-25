
using Assets.Scripts.Objects;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class ThingModel
    {
        public static void HandlePost(Thing thing, ThingPayload payload)
        {
            if (payload.customName != null && payload.customName.Length > 0)
            {
                thing.CustomName = payload.customName;
                thing.IsCustomName = true;
            }

            if (payload.accessState.HasValue)
            {
                thing.AccessState = payload.accessState.Value;
            }
        }
    }
}