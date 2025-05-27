using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Myrmidon.Core.Maps;
using Myrmidon.Core.Entities;
using System.Collections.Generic;

namespace Myrmidon.MonoGame.Rendering {
    public class TileRenderer {
        private readonly SpriteFont _font;
        private readonly Texture2D _tilesheet;
        private readonly int _tileSize;
        private readonly SpriteBatch _spriteBatch;

        public TileRenderer(SpriteBatch spriteBatch, Texture2D tilesheet, int tileSize, SpriteFont font) {
            _spriteBatch = spriteBatch;
            _tilesheet = tilesheet;
            _tileSize = tileSize;
            _font = font;
        }

        public void RenderMap(Map map, Rectangle viewBounds) {
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

        public void RenderEntities(IEnumerable<Entity> entities, Rectangle viewBounds) {
            foreach (var entity in entities) {
                //var pos = entity.Position;
                //if (!viewBounds.Contains(pos)) continue;

                //var screenPos = new Vector2(
                //    (pos.X - viewBounds.Left) * _tileSize,
                //    (pos.Y - viewBounds.Top) * _tileSize
                //);

                //var sourceRect = new Rectangle(entity.Glyph * _tileSize, 0, _tileSize, _tileSize);
                //_spriteBatch.Draw(_tilesheet, screenPos, sourceRect, entity.Color);
            }
        }
    }
}
