using System;

namespace WebAPI.JsonTranslation
{
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonPropertyGetterAttribute : Attribute
    {
        public string PropertyName { get; }
        public JsonPropertyGetterAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}