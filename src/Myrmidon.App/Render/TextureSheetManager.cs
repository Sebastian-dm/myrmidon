using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.App.Render {
    internal class TextureSheetManager {



        private readonly Dictionary<string, TextureSheet> _textureSheets = new Dictionary<string, TextureSheet>();


        public TextureSheetManager() { }

        public TextureSheet LoadTextureSheet(string id, nint renderer) {
            return new TextureSheet(id, renderer);
        }

        public TextureSheet GetTextureSheet(string id, nint renderer) {
            // Check if the texture sheet is already loaded
            if (_textureSheets.ContainsKey(id)) {
                return _textureSheets[id];
            }

            // If not, load it and add it to the dictionary
            var textureSheet = LoadTextureSheet(id, renderer);
            _textureSheets[id] = textureSheet;
            return textureSheet;
        }
    }
}
