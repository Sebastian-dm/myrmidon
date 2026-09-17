using System;
using System.Collections.Generic;

namespace Myrmidon.Core.Ecs;

public sealed class Ecs(uint Size=0) {

    public IEnumerable<EntityId> Entities => _entities;


    private readonly Dictionary<Type, IPartStore> _stores = new Dictionary<Type, IPartStore>();
    private readonly HashSet<EntityId> _entities = new HashSet<EntityId>();
    private uint _nextEntityId = 1;


    public EntityId CreateEntity() {
        var entity = new EntityId(_nextEntityId++);
        _entities.Add(entity);
        return entity;
    }


    public void DestroyEntity(EntityId entity) {
        if (!_entities.Remove(entity))
            return;

        foreach (var store in _stores.Values)
            store.Remove(entity.Value);
    }


    public T Add<T>(EntityId entity, T component) where T : class {
        return Add(entity.Value, component);
    }
    public T Add<T>(uint id, T component) where T : class {
        return (Size == 0) ?
            GetStore<T>().Add(id, component) :
            GetArrStore<T>().Add(id, component);
    }


    public T Get<T>(EntityId entity) where T : class {
        return Get<T>(entity.Value);
    }
    public T Get<T>(uint id) where T : class {
        return (Size==0) ?
            GetStore<T>().Get(id) :
            GetArrStore<T>().Get(id);
    }


    public bool TryGet<T>(EntityId entity, out T? component) where T : class {
        return TryGet<T>(entity.Value, out component);
    }
    public bool TryGet<T>(uint id, out T? component) where T : class {
        return (Size==0) ?
            GetStore<T>().TryGet(id, out component) :
            GetArrStore<T>().TryGet(id, out component);
    }


    public bool Has<T>(EntityId entity) where T : class {
        return Has<T>(entity.Value);
    }
    public bool Has<T>(uint id) where T : class {
        return (Size == 0) ?
            GetStore<T>().Contains(id) :
            GetArrStore<T>().Contains(id);
    }


    public bool Remove<T>(EntityId entity) where T : class {
        return Remove<T>(entity.Value);
    }
    public bool Remove<T>(uint id) where T : class {
        return (Size == 0) ?
            GetStore<T>().Remove(id) :
            GetArrStore<T>().Remove(id);
    }



    private PartStore<T> GetStore<T>()
        where T : class {
        var type = typeof(T);

        if (_stores.TryGetValue(type, out var existing))
            return (PartStore<T>)existing;

        var created = new PartStore<T>();
        _stores[type] = created;
        return created;
    }

    private PartArrStore<T> GetArrStore<T>()
        where T : class {
        var type = typeof(T);

        if (_stores.TryGetValue(type, out var existing))
            return (PartArrStore<T>)existing;

        var created = new PartArrStore<T>(Size);
        _stores[type] = created;
        return created;
    }


    public IEnumerable<EntityId> Query<T1, T2>()
        where T1 : class
        where T2 : class {
        foreach (var entity in _entities) {
            if (Has<T1>(entity) && Has<T2>(entity))
                yield return entity;
        }
    }

    public IEnumerable<EntityId> Query<T1, T2, T3>()
        where T1 : class
        where T2 : class
        where T3 : class {
        foreach (var entity in _entities) {
            if (Has<T1>(entity) &&
                Has<T2>(entity) &&
                Has<T3>(entity)) {
                yield return entity;
            }
        }
    }
}