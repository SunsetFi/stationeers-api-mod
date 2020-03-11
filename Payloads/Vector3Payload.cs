
using UnityEngine;

namespace WebAPI.Payloads
{
    public class Vector3Payload
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static Vector3Payload FromVector3(Vector3 v)
        {
            var item = new Vector3Payload();
            item.x = v.x;
            item.y = v.y;
            item.z = v.z;
            return item;
        }
    }
}