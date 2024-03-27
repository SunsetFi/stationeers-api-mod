
using System;

namespace StationeersWebApi.JsonTranslation
{
    public class ReadOnlyPropertyException : JsonTranslationException
    {
        public ReadOnlyPropertyException(string message) : base(message)
        {
        }
    }
}