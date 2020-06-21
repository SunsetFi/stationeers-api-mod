
using System.Collections.Generic;
using Assets.Scripts.Objects;
using WebAPI.Server.Exceptions;

namespace WebAPI.Payloads.JsonSerializerStrategies
{
    public sealed class ThingJsonSerializerStrategy : ObjectPropertyJsonPayloadStrategy<Thing>
    {
        private IDictionary<string, IObjectPropertyGetterSetter<Thing>> properties = new Dictionary<string, IObjectPropertyGetterSetter<Thing>>
                {
                    {"referenceId", new ObjectPropertyGetterSetter<Thing, string>(
                        thing => thing.ReferenceId.ToString(),
                        (thing, referenceId) => throw new BadRequestException("The referenceId of a thing cannot be changed.")
                    )},
                    {"prefabHash", new ObjectPropertyGetterSetter<Thing, int>(
                        thing => thing.PrefabHash,
                        (thing, prefabHash) => throw new BadRequestException("The prefabHash of a thing cannot be changed.")
                    )},
                    {"health", new ObjectPropertyGetterSetter<Thing, float>(
                        thing => thing.ThingHealth,
                        (thing, health) => thing.ThingHealth = health
                    )},
                    {"customName", new ObjectPropertyGetterSetter<Thing, string>(
                        thing => thing.IsCustomName ? thing.CustomName : null,
                        (thing, customName) => {
                            if (customName == null) {
                                thing.CustomName = null;
                                thing.IsCustomName = false;
                            }
                            else {
                                thing.CustomName = customName;
                                thing.IsCustomName = true;
                            }
                        }
                    )},
                    {"accessState", new ObjectPropertyGetterSetter<Thing, int>(
                        thing => thing.AccessState,
                        (thing, accessState) => thing.AccessState = accessState
                    )}
                };

        protected override IDictionary<string, IObjectPropertyGetterSetter<Thing>> Properties => this.properties;
    }
}