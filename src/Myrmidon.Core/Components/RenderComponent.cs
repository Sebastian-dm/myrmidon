using Myrmidon.Core.Entities;

namespace Myrmidon.Core.Components;

public class RenderComponent {

    //public Entity Entity { get; set; }
    
    public string TextureSheetName { get; set; } = "text/default";
    
    public byte TextureIndex { get; set; } = (byte)'.';
    public byte TextureIndexOriginal {get; set; } = (byte)'.';

    public string ColorBase { get; set; } = "W";
    public string ColorAccent { get; set; } = "R";
    public string ColorBackground { get; set; } = string.Empty;
    
    public bool IsDimmed { get; set; } = false;


    public RenderComponent() {
        //Entity = entity;
    }
    
    public RenderComponent(
        string textureSheetName, byte textureIndex,  string colorBase, string colorAccent = "", string colorBackground = "") {
        //Entity = entity;
        TextureSheetName = textureSheetName;
        TextureIndex = textureIndex;
        ColorBase = colorBase;
        ColorAccent = colorAccent;
        ColorBackground = colorBackground;
    }
    
}