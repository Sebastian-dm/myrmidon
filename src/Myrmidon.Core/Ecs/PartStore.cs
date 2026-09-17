using System.Collections.Generic;

namespace Myrmidon.Core.Ecs;


public interface IPartStore {
    void Remove(EntityId entity);
    bool Contains(EntityId entity);
}


public sealed class PartStore<T> : IPartStore
    where T : class {

    private readonly Dictionary<EntityId, T> _parts = new Dictionary<EntityId, T>();

    public T Add(EntityId entity, T component) {
        _parts[entity] = component;
        return component;
    }

    public bool TryGet(EntityId entity, out T? component) {
        return _parts.TryGetValue(entity, out component);
    }

    public T Get(EntityId entity) {
        return _parts[entity];
    }

    public bool Contains(EntityId entity) {
        return _parts.ContainsKey(entity);
    }

    public bool Remove(EntityId entity) {
        return _parts.Remove(entity);
    }

    void IPartStore.Remove(EntityId entity) {
        Remove(entity);
    }
}