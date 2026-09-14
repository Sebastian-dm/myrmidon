using System.Collections.Generic;

namespace Myrmidon.Core.Ecs;


public interface IComponentStore {
    void Remove(EntityId entity);
    bool Contains(EntityId entity);
}


public sealed class ComponentStore<T> : IComponentStore
    where T : class {

    private readonly Dictionary<EntityId, T> _components = new Dictionary<EntityId, T>();

    public T Add(EntityId entity, T component) {
        _components[entity] = component;
        return component;
    }

    public bool TryGet(EntityId entity, out T? component) {
        return _components.TryGetValue(entity, out component);
    }

    public T Get(EntityId entity) {
        return _components[entity];
    }

    public bool Contains(EntityId entity) {
        return _components.ContainsKey(entity);
    }

    public bool Remove(EntityId entity) {
        return _components.Remove(entity);
    }

    void IComponentStore.Remove(EntityId entity) {
        Remove(entity);
    }
}