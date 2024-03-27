using System;

namespace StationeersWebApi.JsonTranslation
{
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonPropertySetterAttribute : Attribute
    {
        public string PropertyName { get; }
        public JsonPropertySetterAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}