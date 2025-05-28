using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Myrmidon.Core.Entities;
using Myrmidon.Core.Actors;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Graphics;

namespace Myrmidon.Core.GameState {
    public class World {
        public Map Map { get; set; }
        public Player Player { get; set; }

        public bool IsMapGenRequested { get; set; } = true;
        public bool IsMapGenInProgress { get; set; } = false;
        public bool IsEntityGenRequested { get; set; } = false;

        public GoRogue.MultiSpatialMap<Entity> Entities => Map.Entities;

        public World(int width, int height) {
            Map = new Map(width, height);
        }
    }
}