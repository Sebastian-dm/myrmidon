using Myrmidon.Core.Parts;
using System;
using System.Collections.Generic;


namespace Myrmidon.Core.ECS;

public sealed class Ecs(uint size=0) {

    private readonly uint _size = size;

    public IEnumerable<EntityId> Entities => _entities;


    private readonly Dictionary<Type, IPartStore> _stores = new Dictionary<Type, IPartStore>();
    private readonly HashSet<EntityId> _entities = new HashSet<EntityId>();
    private uint _nextEntityId = 0;


    public EntityId CreateEntity() {
        if (_size > 0 && _nextEntityId >= _size) {
            throw new InvalidOperationException("Maximum number of entities reached.");
        }
        var entity = new EntityId(_nextEntityId++);
        _entities.Add(entity);
        return entity;
    }


    public void DestroyEntity(EntityId entity) {
        if (!_entities.Remove(entity))
            return;

        foreach (var store in _stores.Values)
            store.Remove(entity.Id);
    }


    public T Add<T>(EntityId entity, T component) where T : class {
        return (_size == 0) ?
            GetStore<T>().Add(entity.Id, component) :
            GetArrStore<T>().Add(entity.Id, component);
    }


    public T Get<T>(EntityId entity) where T : class {
        return (_size == 0) ?
            GetStore<T>().Get(entity.Id) :
            GetArrStore<T>().Get(entity.Id);
    }


    public bool TryGet<T>(EntityId entity, out T? component) where T : class {
        return (_size == 0) ?
            GetStore<T>().TryGet(entity.Id, out component) :
            GetArrStore<T>().TryGet(entity.Id, out component);
    }


    public bool Has<T>(EntityId entity) where T : class {
        return (_size == 0) ?
            GetStore<T>().Contains(entity.Id) :
            GetArrStore<T>().Contains(entity.Id);
    }


    public bool Remove<T>(EntityId entity) where T : class {
        return (_size == 0) ?
            GetStore<T>().Remove(entity.Id) :
            GetArrStore<T>().Remove(entity.Id);
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