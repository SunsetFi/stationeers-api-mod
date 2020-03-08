
using UnityEngine;

namespace WebAPI.API
{
    public class Vector3Item
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static Vector3Item FromVector3(Vector3 v)
        {
            var item = new Vector3Item();
            item.x = v.x;
            item.y = v.y;
            item.z = v.z;
            return item;
        }
    }
}