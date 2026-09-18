using System;
using System.Collections.Generic;

namespace Myrmidon.Core.ECS;

public sealed class Ecs(uint size=0) {

    private readonly uint _size = size;

    public IEnumerable<EntityId> Entities => _entities;


    private readonly Dictionary<Type, IPartStore> _stores = new Dictionary<Type, IPartStore>();
    private readonly HashSet<EntityId> _entities = new HashSet<EntityId>();
    private uint _nextEntityId = 1;


    public EntityId CreateEntity() {
        var entity = new EntityId(_nextEntityId++);
        _entities.Add(entity);
        if(_size > 0 && _nextEntityId > _size)
            throw new InvalidOperationException("Maximum number of entities reached.");
        return entity;
    }


    public void DestroyEntity(EntityId entity) {
        if (!_entities.Remove(entity))
            return;

        foreach (var store in _stores.Values)
            store.Remove(entity.Value);
    }


    public T Add<T>(EntityId entity, T component) where T : class {
        return (_size == 0) ?
            GetStore<T>().Add(entity.Value, component) :
            GetArrStore<T>().Add(entity.Value, component);
    }


    public T Get<T>(EntityId entity) where T : class {
        return (_size == 0) ?
            GetStore<T>().Get(entity.Value) :
            GetArrStore<T>().Get(entity.Value);
    }


    public bool TryGet<T>(EntityId entity, out T? component) where T : class {
        return (_size == 0) ?
            GetStore<T>().TryGet(entity.Value, out component) :
            GetArrStore<T>().TryGet(entity.Value, out component);
    }


    public bool Has<T>(EntityId entity) where T : class {
        return (_size == 0) ?
            GetStore<T>().Contains(entity.Value) :
            GetArrStore<T>().Contains(entity.Value);
    }


    public bool Remove<T>(EntityId entity) where T : class {
        return (_size == 0) ?
            GetStore<T>().Remove(entity.Value) :
            GetArrStore<T>().Remove(entity.Value);
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

        var created = new PartArrStore<T>(_size);
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