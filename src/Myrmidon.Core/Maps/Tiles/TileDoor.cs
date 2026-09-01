using System;
using Myrmidon.Core.Components;
using Myrmidon.Core.Utilities.Graphics;


namespace Myrmidon.Core.Maps.Tiles {
    public class TileDoor : Tile {

        public bool IsLocked; // Locked door = 1, Unlocked = 0
        public bool IsOpen; // Open door = 1, closed = 0
        

        //Default constructor
        //A TileDoor can be set locked/unlocked/open/closed using the constructor.
        public TileDoor(RenderComponent renderComponent, bool isLocked, bool open) :
            base(renderComponent, open, open) {
            //Update door fields
            IsLocked = isLocked;
            IsOpen = open;

            //change the symbol to open if the door is open
            if (!IsLocked && IsOpen)
                Open();
            else if (IsLocked || !IsOpen)
                Close();
            
            //Hidden by default
            RenderComponent.TextureIndex = 0;
            Name = "standard door";
        }

        //closes a door
        public void Close() {
            RenderComponent.TextureIndex = (byte)'D';
            IsOpen = false;
            IsBlockingLos = true;
            IsWalkable = false;
        }

        //opens a door
        public void Open() {
            IsOpen = true;
            IsBlockingLos = false;
            IsWalkable = true;
            RenderComponent.TextureIndex = (byte)'D';
        }
    }
}