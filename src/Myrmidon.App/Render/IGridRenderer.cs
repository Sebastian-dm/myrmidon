using Bramble.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.App.Render {
    internal interface IGridRenderer {
        void DrawText(Vec location, string text, string color);
        void DrawGlyph(Vec location, byte glyph, string color);
        void DrawTile(
            Vec location,
            string textureSheet,
            byte index,
            string foreground,
            string accent,
            string? background = null);
    }
}
