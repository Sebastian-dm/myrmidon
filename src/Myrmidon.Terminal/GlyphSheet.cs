using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

using Malison.Core;
using SDL3;

using static System.Net.Mime.MediaTypeNames;

namespace Myrmidon.Terminal {

    public class GlyphSheet {

        public string Id { get; private set; }


        public static GlyphSheet GetGlyphSheet(string id = "Default") {
            var glyphSheet = sGlyphSheets.FirstOrDefault(gs => gs.Id == id);

            if (glyphSheet == null) {
                glyphSheet = new GlyphSheet(id);
            }
            return glyphSheet;
        }


        public int Width { get { return mBitmap.Width / GlyphsPerRow; } }
        public int Height { get { return mBitmap.Height / GlyphRows; } }

        public Bitmap GetBitmap(Character character) {

            // use the previously cached one if there
            Bitmap bitmap = null;
            if (mCharacterCache.TryGetValue(character, out bitmap)) {
                return bitmap;
            }

            // not there, so create it
            Bitmap characterBitmap = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(characterBitmap)) {
                byte glyph = (byte)character.Glyph;
                int column = glyph % GlyphsPerRow;
                int row = glyph / GlyphsPerRow;

                Rectangle destRect = new Rectangle(0, 0, Width, Height);

                ColorMap map = new ColorMap();
                map.OldColor = Color.Black;
                map.NewColor = character.ForeColor.ToSystemColor();

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetRemapTable(new ColorMap[] { map });

                g.DrawImage(mBitmap, destRect,
                    column * Width, row * Height, Width, Height,
                    GraphicsUnit.Pixel, attributes);
            }

            // cache it
            mCharacterCache[character] = characterBitmap;

            return characterBitmap;
        }

        public void Draw(Graphics g, int x, int y, Character character) {
            // don't draw if it's a blank glyph
            if (character.Glyph == Glyph.Space) return;

            Bitmap characterBitmap = GetBitmap(character);

            Rectangle destRect = new Rectangle(x, y, Width, Height);
            g.DrawImageUnscaledAndClipped(characterBitmap, destRect);
        }


        private GlyphSheet(string id) {
            Id = id;
            string configText = File.ReadAllText($"images/{id}.json");
            var config = JsonSerializer.Deserialize<GlyphSheetConfig>(configText);
            GlyphsPerRow = config.GlyphsPerRow;
            GlyphRows = config.GlyphRows;
            AsciiOffset = config.AsciiOffset;

            string bitMapFileName = $"images/{id}.png";
            mBitmap = new Bitmap(bitMapFileName);
            mCharacterCache = new Dictionary<Character, Bitmap>();
        }
        private int GlyphsPerRow { get; set; }
        private int GlyphRows { get; set; }
        private int AsciiOffset { get; set; }

        private static List<GlyphSheet> sGlyphSheets = new List<GlyphSheet> { };

        private Bitmap mBitmap;
        private Dictionary<Character, Bitmap> mCharacterCache;
    }


    public class GlyphSheetConfig {
        public int GlyphWidth { get; set; }
        public int GlyphHeight { get; set; }
        public int GlyphsPerRow { get; set; } = 32;
        public int GlyphRows { get; set; } = 6;
        public int AsciiOffset { get; set; } = 32; // default to ASCII offset

    }
}
