
using System;

namespace WebAPI.JsonTranslation
{
    public class JsonTranslationException : Exception
    {
        public JsonTranslationException(string message) : base(message)
        {
        }
    }
}