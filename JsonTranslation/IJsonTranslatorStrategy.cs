using System;
using Newtonsoft.Json.Linq;

namespace WebAPI.JsonTranslation
{
    public interface IJsonTranslatorStrategy
    {
        Type TargetType { get; }
        string[] SupportedProperties { get; }

        void WriteObjectToJson(object target, JObject output);

        void VerifyJsonUpdate(object target, JObject input);
        void UpdateObjectFromJson(object target, JObject input);
    }
}