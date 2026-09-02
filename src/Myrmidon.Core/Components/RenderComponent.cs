using Myrmidon.Core.Entities;

namespace Myrmidon.Core.Components;

public class RenderComponent {

    public string SpriteId { get; set; } = "default.floor";
    public string TextureSheetName { get; set; } = "text/default";
    
    public byte TextureIndex { get; set; } = (byte)'.';
    public int VariantOffset { get; set; } = 0;
    public int AnimationOffset { get; set; } = 0;

    public string ColorBase { get; set; } = "W";
    public string ColorAccent { get; set; } = "R";
    public string ColorBackground { get; set; } = string.Empty;
    

    public int Layer { get; set; } = 0;
    public bool Visible { get; set; } = true;
    public bool Explored { get; set; } = false;
    public bool Dirty { get; set; } = false;


    public RenderComponent() {
    }
    
    public RenderComponent(
        string textureSheetName, byte textureIndex,  string colorBase, string colorAccent = "", string colorBackground = "") {
        TextureSheetName = textureSheetName;
        TextureIndex = textureIndex;
        ColorBase = colorBase;
        ColorAccent = colorAccent;
        ColorBackground = colorBackground;
    }
    
}