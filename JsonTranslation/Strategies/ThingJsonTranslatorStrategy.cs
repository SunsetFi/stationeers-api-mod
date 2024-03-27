
using Assets.Scripts.Objects;

namespace StationeersWebApi.JsonTranslation.Strategies
{
    [JsonTranslatorStrategy]
    [JsonTranslatorTarget(typeof(Thing))]
    public sealed class ThingJsonTranslatorStrategy
    {
        [JsonPropertyGetter("referenceId")]
        public string GetReferenceId(Thing thing)
        {
            return thing.ReferenceId.ToString();
        }

        [JsonPropertyGetter("prefabHash")]
        public int GetPrefabHash(Thing thing)
        {
            return thing.PrefabHash;
        }

        [JsonPropertyGetter("prefabName")]
        public string GetPrefabName(Thing thing)
        {
            return thing.PrefabName;
        }

        [JsonPropertyGetter("health")]
        public float GetHealth(Thing thing)
        {
            return thing.ThingHealth;
        }

        [JsonPropertySetter("health")]
        public void SetHealth(Thing thing, float health)
        {
            thing.ThingHealth = health;
        }

        [JsonPropertyGetter("customName")]
        public string GetCustomName(Thing thing)
        {
            return thing.IsCustomName ? thing.CustomName : null;
        }

        [JsonPropertySetter("customName")]
        public void SetCustomName(Thing thing, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                thing.IsCustomName = false;
                thing.CustomName = null;
            }
            else
            {
                thing.CustomName = name;
                thing.IsCustomName = true;
            }
        }

        [JsonPropertyGetter("accessState")]
        public int GetAccessState(Thing thing)
        {
            return thing.AccessState;
        }

        [JsonPropertySetter("accessState")]
        public void SetAccessState(Thing thing, int accessState)
        {
            thing.AccessState = accessState;
        }
    }
}