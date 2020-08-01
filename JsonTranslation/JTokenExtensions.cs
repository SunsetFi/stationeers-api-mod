using System;
using Newtonsoft.Json.Linq;

namespace WebAPI.JsonTranslation
{
    public static class JObjectExtensions
    {
        public static JToken GetProperty(this JObject obj, string property)
        {
            if (!obj.ContainsKey(property))
            {
                throw new ArgumentException($"Property \"{property}\" not found.");
            }
            return obj[property];
        }

        public static void VerifyIntegerProperty(this JObject obj, string property)
        {
            if (!obj.ContainsKey(property))
            {
                return;
            }
            if (obj[property].Type != JTokenType.Integer)
            {
                throw new PropertyTypeException($"Expected property \"{property}\" to be an integer.");
            }
        }

        public static int GetIntegerProperty(this JObject obj, string property)
        {
            var value = obj.GetProperty(property);
            if (value.Type != JTokenType.Integer)
            {
                throw new PropertyTypeException($"Expected property \"{property}\" to be an integer.");
            }
            return value.ToObject<int>();
        }

        public static void VerifyStringProperty(this JObject obj, string property, bool nullable = false)
        {
            if (!obj.ContainsKey(property))
            {
                return;
            }
            if (obj[property].Type != JTokenType.String && (nullable == false || obj[property].Type != JTokenType.Null))
            {
                throw new PropertyTypeException($"Expected property \"{property}\" to be a string.");
            }
        }

        public static string GetStringProperty(this JObject obj, string property)
        {
            var value = obj.GetProperty(property);
            if (value.Type != JTokenType.String && value.Type != JTokenType.Null)
            {
                throw new PropertyTypeException($"Expected property \"{property}\" to be a string.");
            }
            return value.ToObject<string>();
        }


    }
}