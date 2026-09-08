using Myrmidon.Core.Entities;

namespace Myrmidon.Core.Components;

public class RenderComponent {

    public string SpriteId { get; set; } = "default.floor";
    public string TextureSheetName { get; set; } = "text/default";
    
    public byte TextureIndex { get; set; } = (byte)'.';
    private byte _textureIndex { get; set; } = (byte)'.';
    public int VariantOffset { get; set; } = 0;
    public int AnimationOffset { get; set; } = 0;

    public string ColorBase { get; set; } = "W";
    public string ColorAccent { get; set; } = "R";
    public string ColorBackground { get; set; } = string.Empty;
    

    public int Layer { get; set; } = 0;
    public float Dimfactor { get; set; } = 0f;
    public bool Explored { get; set; } = false;
    public bool Dirty { get; set; } = false;


    public RenderComponent() {
    }
    
    public RenderComponent(
        string textureSheetName, byte textureIndex,  string colorBase, string colorAccent = "", string colorBackground = "") {
        TextureSheetName = textureSheetName;
        TextureIndex = textureIndex;
        _textureIndex = textureIndex;
        ColorBase = colorBase;
        ColorAccent = colorAccent;
        ColorBackground = colorBackground;
    }

    public void ResetVariant() {
        VariantOffset = 0;
        TextureIndex = _textureIndex;
    }

    public void SetVariant(byte n) {
        VariantOffset = n;
        TextureIndex = (byte)(_textureIndex + n);
    }


}