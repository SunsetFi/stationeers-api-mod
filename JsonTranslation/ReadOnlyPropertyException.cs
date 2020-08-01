
using System;

namespace WebAPI.JsonTranslation
{
    public class ReadOnlyPropertyException : JsonTranslationException
    {
        public ReadOnlyPropertyException(string message) : base(message)
        {
        }
    }
}