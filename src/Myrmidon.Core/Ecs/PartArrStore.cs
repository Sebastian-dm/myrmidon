using System.Collections.Generic;

namespace Myrmidon.Core.ECS;



public sealed class PartArrStore<T>(uint size) : IPartStore
    where T : class {

    private readonly T?[] _parts = new T?[size];

    public T Add(uint id, T component) {
        _parts[id] = component;
        return component;
    }

    public T Get(uint id) {
        return _parts[id]!;
    }

    public bool TryGet(uint id, out T? component) {
        component = _parts[id];
        return component != null;
    }

    public bool Contains(uint id) {
        return _parts[id] != null;
    }

    public bool Remove(uint id) {
        _parts[id] = null;
        return true;
    }

    void IPartStore.Remove(uint id) {
        Remove(id);
    }
}