using System;

namespace StationeersWebApi.JsonTranslation
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