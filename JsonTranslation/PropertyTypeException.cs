
using System;

namespace WebAPI.JsonTranslation
{
    public class PropertyTypeException : JsonTranslationException
    {
        public PropertyTypeException(string message) : base(message)
        {
        }
    }
}