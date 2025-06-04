using Bramble.Core;
using GoRogue;
using Malison.Core;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Myrmidon.App.UI {
    internal class TileRenderer {

        public void Paint(ITerminal terminal, IGameContext context) {
            
            terminal.Clear();

            var map = context.World.Map;
            if (map == null) return;

            Vec center = new Vec(context.World.Player.Position.X, context.World.Player.Position.Y);

            Rect viewBounds = new Rect(center - terminal.Size/2, terminal.Size);

            // Paint tiles
            for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
                for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
                    if (!IsInMapBounds(x, y, map)) continue;

                    var tile = map.GetTileAt<Tile>(x, y);

                    var screenPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);
                    terminal[screenPos.X, screenPos.Y][TermColor.Gray, TermColor.Black].Write(tile.Glyph);

                }
            }

            //Paint entities
            foreach (var entity in map.Entities.Items) {
                if (entity == null)
                    Debug.WriteLine("Null entity in map.Entities!");
                else
                    Debug.WriteLine(entity.GetType().FullName);

                if (entity is Actor actor) {
                    if (!IsInMapBounds(actor.Position.X, actor.Position.Y, map)) continue;
                    var screenPos = new Vec(actor.Position.X - viewBounds.Left, actor.Position.Y - viewBounds.Top);
                    terminal[screenPos.X, screenPos.Y][TermColor.LightRed, TermColor.DarkGray].Write(actor.Glyph);
                }
            }
        }

        private bool IsInMapBounds(int x, int y, TileMap map) {

            bool a = x >= map.Bounds.Left;
            bool b = x < map.Bounds.Right;
            bool c = y >= map.Bounds.Top;
            bool d = y < map.Bounds.Bottom;

            if (x > 0 && y > 0) {
                var g = "";
            }

            return x >= map.Bounds.Left && x < map.Bounds.Right && y >= map.Bounds.Top && y < map.Bounds.Bottom;
        }
    }
}
