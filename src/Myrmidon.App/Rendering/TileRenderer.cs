using Bramble.Core;
using GoRogue;
using Malison.Core;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Myrmidon.App.Rendering {
    internal class TileRenderer {

        public void Paint(ITerminal terminal) {
            terminal.Clear();

            for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
                for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
                    //if (!map.IsInBounds(x, y)) continue;

                    //var tile = map.GetTile(x, y);

                    //var screenPos = new Vector2(
                    //    (x - viewBounds.Left) * _tileSize,
                    //    (y - viewBounds.Top) * _tileSize
                    //);

                    //var sourceRect = new Rectangle(tile.Glyph * _tileSize, 0, _tileSize, _tileSize);
                    //_spriteBatch.Draw(_tilesheet, screenPos, sourceRect, tile.ForegroundColor);
                }
            }
        }
    }
}
