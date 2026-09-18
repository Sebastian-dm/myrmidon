using Bramble.Core;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Utilities.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;


namespace Myrmidon.Core.Zones;


public class Zone
{

    public int Id { get; set; }
    public TileMap Map { get; set; }
    public ZoneSpatialIndex SpatialIndex { get; }
    public ZoneGenState GenerationState { get; set; } = ZoneGenState.NotStarted;



    public Zone(int id, int width, int height)
    {
        Id = id;
        Map = new TileMap(width, height);
        SpatialIndex = new ZoneSpatialIndex();
    }


}


public sealed class ZoneSpatialIndex
{
    private readonly Dictionary<Vec, HashSet<EntityId>> _entitiesByPosition = new();

    public void Add(EntityId entity, Vec position) {
        if (!_entitiesByPosition.TryGetValue(position, out var entities)) {
            entities = new HashSet<EntityId>();
            _entitiesByPosition[position] = entities;
        }

        entities.Add(entity);
    }

    public void Remove(EntityId entity, Vec position) {
        if (!_entitiesByPosition.TryGetValue(position, out var entities))
            return;

        entities.Remove(entity);

        if (entities.Count == 0)
            _entitiesByPosition.Remove(position);
    }

    public IReadOnlyCollection<EntityId> At(Vec position) {
        return _entitiesByPosition.TryGetValue(position, out var entities)
            ? entities
            : [];
    }

    public IEnumerable<EntityId> All() {
        return _entitiesByPosition.Values.SelectMany(hashSet => hashSet);
    }

    public IEnumerable<EntityId> InBounds(Rect bounds) {
        for (int y = bounds.Top; y < bounds.Bottom; y++) {
            for (int x = bounds.Left; x < bounds.Right; x++) {
                var position = new Vec(x, y);

                if (_entitiesByPosition.TryGetValue(position, out var entities)) {
                    foreach (var entity in entities)
                        yield return entity;
                }
            }
        }
    }
}