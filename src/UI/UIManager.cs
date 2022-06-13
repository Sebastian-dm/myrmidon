using System;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace clodd.UI {

    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole {
        
        public ScrollingConsole MapConsole;
        public Window MapWindow;
        public MessageLogWindow MessageLog;

        public UIManager() {
            // must be set to true or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            // The UIManager becomes the only screen that SadConsole processes
            Parent = SadConsole.Global.CurrentScreen;
        }



        /// <summary>
        /// Initializes all consoles and windows
        /// </summary>
        public void Init() {

            // Set square font
            FontMaster fontMaster = SadConsole.Global.LoadFont("res/fonts/Square_12x12.font");
            Font normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
            SadConsole.Global.FontDefault = normalSizedFont;

            CreateConsoles();

            // Add map windows
            CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Game Map");

            //Add message log window
            MessageLog = new MessageLogWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Message Log");
            Children.Add(MessageLog);
            MessageLog.Show();
            MessageLog.Position = new Point(0, GameLoop.GameHeight / 2);

            // TEST CODE
            MessageLog.Add(System.IO.Directory.GetCurrentDirectory());
        }




        /// <summary>
        /// Creates all child consoles to be managed
        /// </summary>
        public void CreateConsoles() {
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), GameLoop.World.CurrentMap.Tiles);
        }



        /// <summary>
        /// centers the viewport camera on an Actor
        /// </summary>
        /// <param name="actor"></param>
        public void CenterOnActor(Actor actor) {
            MapConsole.CenterViewPortOnPoint(actor.Position);
        }



        /// <summary>
        /// Overrides the Containerconsole's update by inserting the inputhandler and calling the base method
        /// </summary>
        /// <param name="timeElapsed"></param>
        public override void Update(TimeSpan timeElapsed) {
            InputHandler.Update();
            base.Update(timeElapsed);
        }


        // Creates a window that encloses a map console
        // of a specified height and width
        // and displays a centered window title
        // make sure it is added as a child of the UIManager
        // so it is updated and drawn
        public void CreateMapWindow(int width, int height, string title) {
            
            MapWindow = new Window(width, height);
            MapWindow.CanDrag = true;

            //make console short enough to show the window title
            //and borders, and position it away from borders
            int mapConsoleWidth = width - 2;
            int mapConsoleHeight = height - 2;

            // Resize the Map Console's ViewPort to fit inside of the window's borders snugly
            MapConsole.ViewPort = new Rectangle(0, 0, mapConsoleWidth, mapConsoleHeight);

            //reposition the MapConsole so it doesnt overlap with the left/top window edges
            MapConsole.Position = new Point(1, 1);

            //close window button
            Button closeButton = new Button(3, 1);
            closeButton.Position = new Point(0, 0);
            closeButton.Text = "[X]";

            //Add the close button to the Window's list of UI elements
            MapWindow.Add(closeButton);

            // Centre the title text at the top of the window
            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);

            //add the map console to the window
            MapWindow.Children.Add(MapConsole);

            // The MapWindow becomes a child console of the UIManager
            Children.Add(MapWindow);

            // Add the player to the MapConsole's render list
            MapConsole.Children.Add(GameLoop.World.Player);

            // Without this, the window will never be visible on screen
            MapWindow.Show();
        }
    }
}