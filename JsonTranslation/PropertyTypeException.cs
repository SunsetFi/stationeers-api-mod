
using System;

namespace StationeersWebApi.JsonTranslation
{
    public class PropertyTypeException : JsonTranslationException
    {
        public PropertyTypeException(string message) : base(message)
        {
        }
    }
}