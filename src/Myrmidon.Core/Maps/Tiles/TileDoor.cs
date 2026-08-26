using System;

using Myrmidon.Core.Utilities.Graphics;


namespace Myrmidon.Core.Maps.Tiles {
    public class TileDoor : Tile {

        public bool IsLocked; // Locked door = 1, Unlocked = 0
        public bool IsOpen; // Open door = 1, closed = 0
        

        //Default constructor
        //A TileDoor can be set locked/unlocked/open/closed using the constructor.
        public TileDoor(bool isLocked, bool open) : base(new Color(100, 60, 20), new Color(20, 10, 0), 254) {
            //Update door fields
            IsLocked = isLocked;
            IsOpen = open;

            //change the symbol to open if the door is open
            if (!IsLocked && IsOpen)
                Open();
            else if (IsLocked || !IsOpen)
                Close();
            
            //Hidden by default
            Glyph = 0;
            Name = "standard door";
        }

        //closes a door
        public void Close() {
            IsOpen = false;
            IsBlockingLOS = true;
            IsWalkable = false;
            Glyph = 254;
        }

        //opens a door
        public void Open() {
            IsOpen = true;
            IsBlockingLOS = false;
            IsWalkable = true;
            Glyph = (byte)'+';
        }
    }
}