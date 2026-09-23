using System.Collections.Generic;
using Myrmidon.Core.Parts;

namespace Myrmidon.Core.ECS;


public sealed class PartStore<T> : IPartStore where T : Part
{

    private readonly Dictionary<uint, T> _parts = new Dictionary<uint, T>();

    public T Add(uint id, T part)
    {
        _parts[id] = part;
        return part;
    }

    public bool TryGet(uint id, out T? part)
    {
        return _parts.TryGetValue(id, out part);
    }

    public T Get(uint id)
    {
        return _parts[id];
    }

    public bool Contains(uint id)
    {
        return _parts.ContainsKey(id);
    }

    public bool Remove(uint id)
    {
        return _parts.Remove(id);
    }

    void IPartStore.Remove(uint id)
    {
        Remove(id);
    }
}