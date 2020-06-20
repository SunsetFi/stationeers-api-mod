
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Objects;
using WebAPI.Payloads;

namespace WebAPI.Models
{
    public static class ItemsModel
    {
        public static IList<ItemPayload> GetItems()
        {
            // AllDevices has duplicates, so filtering this to be safe.
            var set = new HashSet<Item>();
            foreach (var item in Item.AllItems)
            {
                set.Add(item);
            }
            return set.Select(x => ItemPayload.FromItem(x)).ToList();
        }
    }
}