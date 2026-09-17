using System.Collections.Generic;

namespace Myrmidon.Core.Ecs;


public sealed class PartStore<T> : IPartStore
    where T : class {

    private readonly Dictionary<uint, T> _parts = new Dictionary<uint, T>();

    public T Add(uint id, T component) {
        _parts[id] = component;
        return component;
    }

    public bool TryGet(uint id, out T? component) {
        return _parts.TryGetValue(id, out component);
    }

    public T Get(uint id) {
        return _parts[id];
    }

    public bool Contains(uint id) {
        return _parts.ContainsKey(id);
    }

    public bool Remove(uint id) {
        return _parts.Remove(id);
    }

    void IPartStore.Remove(uint id) {
        Remove(id);
    }
}