using System;
using Newtonsoft.Json.Linq;

namespace WebAPI.Payloads
{
    public interface IJsonPayloadStrategy
    {
        Type TargetType { get; }
        string[] SupportedProperties { get; }

        void WriteObjectToPayload(object target, JObject output);
        void UpdateObjectFromPayload(object target, JObject input);
    }
}