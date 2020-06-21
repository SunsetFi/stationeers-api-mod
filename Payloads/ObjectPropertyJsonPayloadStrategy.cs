
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace WebAPI.Payloads
{
    public interface IObjectPropertyGetterSetter<TTarget>
    {
        Type PropertyType { get; }
        JToken GetProperty(TTarget target);
        void SetProperty(TTarget target, JToken value);
    }

    // TODO: Some properties are read-only.  We should validate all write requests before writing anything
    //  Otherwise, we can partially update an object but return BadRequest when we get to the bad property.
    public class ObjectPropertyGetterSetter<TTarget, TValueType> : IObjectPropertyGetterSetter<TTarget>
    {
        private Func<TTarget, TValueType> getter;
        private Action<TTarget, TValueType> setter;

        public Type PropertyType => typeof(TValueType);

        public ObjectPropertyGetterSetter(Func<TTarget, TValueType> getter, Action<TTarget, TValueType> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public JToken GetProperty(TTarget target)
        {
            return JToken.FromObject(this.getter(target));
        }

        public void SetProperty(TTarget target, JToken value)
        {
            this.setter(target, value.ToObject<TValueType>());
        }
    }

    public abstract class ObjectPropertyJsonPayloadStrategy<TTarget> : IJsonPayloadStrategy
    {
        protected abstract IDictionary<string, IObjectPropertyGetterSetter<TTarget>> Properties { get; }

        public Type TargetType => typeof(TTarget);

        public string[] SupportedProperties => this.Properties.Keys.ToArray();

        public void UpdateObjectFromPayload(object target, JObject input)
        {
            // Callers should check TargetType before calling this.
            var typedTarget = (TTarget)target;

            foreach (var propertyName in input.Properties().Select(x => x.Name))
            {
                IObjectPropertyGetterSetter<TTarget> getterSetter;
                if (!this.Properties.TryGetValue(propertyName, out getterSetter))
                {
                    continue;
                }
                getterSetter.SetProperty(typedTarget, input[propertyName]);
            }
        }

        public void WriteObjectToPayload(object target, JObject output)
        {
            // Callers should check TargetType before calling this.
            var typedTarget = (TTarget)target;

            foreach (var property in this.Properties)
            {
                output[property.Key] = property.Value.GetProperty(typedTarget);
            }
        }
    }
}