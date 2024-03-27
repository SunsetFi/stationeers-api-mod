
using System;

namespace StationeersWebApi.JsonTranslation
{
    public class JsonTranslationException : Exception
    {
        public JsonTranslationException(string message) : base(message)
        {
        }
    }
}