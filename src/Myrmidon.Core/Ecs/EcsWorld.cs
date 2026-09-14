using System;
using System.Collections.Generic;

namespace Myrmidon.Core.Ecs;

public sealed class EcsWorld {

    public IEnumerable<EntityId> Entities => _entities;


    private readonly Dictionary<Type, IComponentStore> _stores = new Dictionary<Type, IComponentStore>();
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
            store.Remove(entity);
    }

    public T Add<T>(EntityId entity, T component)
        where T : class {
        var store = GetStore<T>();
        return store.Add(entity, component);
    }

    public T Get<T>(EntityId entity)
        where T : class {
        return GetStore<T>().Get(entity);
    }

    public bool TryGet<T>(EntityId entity, out T? component)
        where T : class {
        return GetStore<T>().TryGet(entity, out component);
    }

    public bool Has<T>(EntityId entity)
        where T : class {
        return GetStore<T>().Contains(entity);
    }

    public bool Remove<T>(EntityId entity)
        where T : class {
        return GetStore<T>().Remove(entity);
    }


    private ComponentStore<T> GetStore<T>()
        where T : class {
        var type = typeof(T);

        if (_stores.TryGetValue(type, out var existing))
            return (ComponentStore<T>)existing;

        var created = new ComponentStore<T>();
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