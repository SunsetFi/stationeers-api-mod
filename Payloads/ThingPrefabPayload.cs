using System.Collections.Generic;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.Util;
using Newtonsoft.Json;

namespace WebAPI.Payloads
{
    public class ThingPrefabPayload
    {
        public Dictionary<string, ThingPayload> things { get; set; }

        public static ThingPrefabPayload FromGame()
        {
            var item = new ThingPrefabPayload();
            item.things = new Dictionary<string, ThingPayload>();

            foreach (Thing thing in Thing.AllPrefabs)
            {
                var payload = ThingPayload.FromThing(thing);
                item.things[thing.name] = payload;
            }

            return item;
        }

        public class ThingPayload 
        {
            [JsonIgnore]
            public string name { get; set; }
            public int hash { get; set; }
            public int stackSize { get; set; }
            [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
            public Nutrition nutrition { get; set; }
            [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
            public Meltable meltable { get; set; }

            public static ThingPayload FromThing(Thing prefab)
            {
                var item = new ThingPayload();

                item.name = prefab.name;
                item.hash = prefab.GetPrefabHash;
                
                if (prefab is IQuantity quantity) {
                    item.stackSize = (int) quantity.GetMaxQuantity;
                } else {
                    item.stackSize = 1;
                }

                if (prefab is INutrition nutrition) {
                    item.nutrition = Nutrition.FromINutrition(nutrition);
                }

                if (prefab is Ice ice)
                {
                    item.meltable = Meltable.FromIce(ice);
                }
                
                return item;
            }
        }

        public class Nutrition {
            public float perQuantity { get; set; }

            public static Nutrition FromINutrition(INutrition iNutrition)
            {
                var nutrition = new Nutrition();
                nutrition.perQuantity = iNutrition.Nutrition(1f);

                return nutrition;
            }
        }

        public class Meltable {
            public float temperature { get; set; }
            public float pressure { get; set; }

            public static Meltable FromIce(Ice ice)
            {
                var meltable = new Meltable();
                meltable.temperature = ice.MeltTemperature;
                meltable.pressure = ice.MeltPressure;
                
                return meltable;
            }
        }
    }
}