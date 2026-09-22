using System;
using System.Linq;
using System.Collections.Generic;

using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.ECS;

namespace Myrmidon.Core.Zones;

// Stores and queries Tile data
public class TileMap {
    

    public List<Rect> Rooms { get; set; }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public Rect Bounds {get {return new Rect(0, 0, Width, Height);}}
        
    public EntityId[] Tiles { get; private set; }
    public EntityId this[int x, int y] {
        get => Tiles[x + y * Width];
        set => Tiles[x + y * Width] = value;
    }
    public EntityId this[int i] {
        get => Tiles[i];
        set => Tiles[i] = value;
    }
    public EntityId this[Vec v] {
        get => Tiles[v.X + v.Y * Width];
        set => Tiles[v.X + v.Y * Width] = value;
    }
    public Ecs Ecs { get; private set; } = new Ecs();


    // Build a new map with a specified width and height
    public TileMap(int width, int height) {
        Width = width;
        Height = height;
        Ecs = new Ecs((uint)(width * height));
        Tiles = new EntityId[width * height];
        for (int i = 0; i < width * height; i++) {
            Tiles[i] = Ecs.CreateEntity();
        }

        Rooms = new List<Rect>();
    }


    
    public bool IsTileWalkable(Vec location) {
        if (!Bounds.Contains(location))
            return false;
        
        if (Ecs.Has<Physics>(Tiles[location.Y * Width + location.X]))
            return Ecs.Get<Physics>(Tiles[location.Y * Width + location.X]).BlocksMovement == false;

        return false;
    }



    // Returns a tile if it exists at location. Return null otherwise.
    public EntityId? GetTileAt(Vec location) {
        return GetTileAt(location.X, location.Y);
    }
    public EntityId? GetTileAt(int x, int y){
        int locationIndex = x + y * Width;
        // make sure the index is within the boundaries of the map!
        if (0 <= locationIndex && locationIndex < Width * Height)
            return Tiles[locationIndex];
        else return null;
    }



    public EntityId?[] GetOrthoAdjacentTiles(Vec loc) {
        return GetOrthoAdjacentTiles(loc.X, loc.Y);
    }
    public EntityId?[] GetOrthoAdjacentTiles(int x, int y) {
        int w = Width;
        int h = Height;

        EntityId?[] result = [
            (               y <= 0  ) ? null : GetTileAt(x  , y-1),
            (x >= w-1               ) ? null : GetTileAt(x+1, y  ),
            (               y >= h-1) ? null : GetTileAt(x  , y+1),
            (x <= 0                 ) ? null : GetTileAt(x-1, y  ),
        ];
        return result;
    }



    // Checks if a specific type of tile at a specified location is on the map. If it exists, returns that Tile.
    public EntityId?[] GetAdjacentTiles(Vec loc) {
        return GetAdjacentTiles(loc.X, loc.Y);
    }
    public EntityId?[] GetAdjacentTiles(int x, int y) {
        int w = Width;
        int h = Height;

        EntityId?[] result = [
            (x <= 0   | y <= 0  ) ? null : GetTileAt(x-1, y-1),
            (           y <= 0  ) ? null : GetTileAt(x  , y-1),
            (x >= w-1 | y <= 0  ) ? null : GetTileAt(x+1, y-1),
            (x >= w-1           ) ? null : GetTileAt(x+1, y  ),
            (x >= w-1 | y >= h-1) ? null : GetTileAt(x+1, y+1),
            (           y >= h-1) ? null : GetTileAt(x  , y+1),
            (x <= 0   | y >= h-1) ? null : GetTileAt(x-1, y+1),
            (x <= 0             ) ? null : GetTileAt(x-1, y  ),
        ];
        return result;
    }


}