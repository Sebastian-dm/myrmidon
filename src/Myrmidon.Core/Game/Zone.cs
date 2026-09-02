using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Graphics;

namespace Myrmidon.Core.Game {
    public class Zone {

        public TileMap Map { get; set; }
        //public Player Player { get; set; }


        public GoRogue.MultiSpatialMap<Entity> Entities => Map.Entities;

        public Zone(int width, int height) {
            Map = new TileMap(width, height);
        }


        public ZoneGenState GenerationState { get; set; } = ZoneGenState.NotStarted;
        public enum ZoneGenState {
            NotStarted,
            GeneratingTerrain,
            Populating,
            Ready
        }
    }
}