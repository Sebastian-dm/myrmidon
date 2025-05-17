﻿using System;
using myrmidon.Entities;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

using myrmidon.Maps;
using myrmidon.Actions;
using myrmidon.Geometry;

namespace myrmidon.UI {

    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole {

        public ScrollingConsole MapConsole;
        public Window MapWindow;
        public MessageLogWindow MessageLog;
        public SadConsole.Themes.Colors CustomColors;
        public bool WaitingForInput = true;

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

            SetupCustomColors();

            CreateConsoles();

            //Message Log initialization
            MessageLog = new MessageLogWindow(Program.GameWidth, Program.GameHeight /4, "Message Log");
            Children.Add(MessageLog);
            MessageLog.Show();
            MessageLog.Position = new Point(0, Program.GameHeight * 3 / 4);

            // TEST CODE
            MessageLog.Add("Started game from path: "+System.IO.Directory.GetCurrentDirectory());

            // Load the map into the MapConsole
            LoadMap(Program.World.CurrentMap);

            // Now that the MapConsole is ready, build the Window
            CreateMapWindow(Program.GameWidth, Program.GameHeight *3/4, "Game Map");
            UseMouse = true;

            // Start the game with the camera focused on the player
            //CenterOnActor(GameLoop.World.Player);
        }


        /// <summary>
        /// Build a new coloured theme based on SC's default theme and then set it as the program's default theme.
        /// </summary>
        private void SetupCustomColors() {
            CustomColors = SadConsole.Themes.Colors.CreateDefault();
            
            Color backgroundColor = Color.Black;
            CustomColors.ControlHostBack = backgroundColor;
            CustomColors.ControlBack = backgroundColor;
            CustomColors.ControlBackLight = (backgroundColor * 1.3f).FillAlpha();
            CustomColors.ControlBackDark = (backgroundColor * 0.7f).FillAlpha();
            CustomColors.ControlBackSelected = CustomColors.GrayDark;
            
            CustomColors.RebuildAppearances();
            SadConsole.Themes.Library.Default.Colors = CustomColors;
        }




        /// <summary>
        /// Creates all child consoles to be managed
        /// </summary>
        public void CreateConsoles() {
            // Temporarily create a console with *no* tile data that will later be replaced with map data
            //FontMaster fontMaster = SadConsole.Global.LoadFont("fonts/Square_16x16.font");
            //Font GraphicFont = fontMaster.GetFont(Font.FontSizes.One);
            MapConsole = new ScrollingConsole(Program.GameWidth,Program.GameHeight);
            //MapConsole.Font = GraphicFont;
        }


        /// <summary>
        /// Loads a Map into the MapConsole
        /// </summary>
        /// <param name="map"></param>
        private void LoadMap(Maps.Map map) {
            // First load the map's tiles into the console
            MapConsole = new SadConsole.ScrollingConsole(
                Program.World.CurrentMap.Width,
                Program.World.CurrentMap.Height,
                Global.FontDefault,
                new Rectangle(0,0,
                    Program.GameWidth,
                    Program.GameHeight
                    ),
                map.Tiles);

            // Now Sync all of the map's entities
            SyncMapEntities(map);
        }


        public void RefreshConsole() {
            MapConsole.SetRenderCells();
            MapConsole.IsDirty = true;
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
            //MapConsole.Children.Add(GameLoop.World.Player);

            // Without this, the window will never be visible on screen
            MapWindow.Show();
        }


        // Adds the entire list of entities found in the
        // World.CurrentMap's Entities SpatialMap to the
        // MapConsole, so they can be seen onscreen
        private void SyncMapEntities(Maps.Map map) {
            // remove all Entities from the console first
            MapConsole.Children.Clear();

            // Now pull all of the entities into the MapConsole in bulk
            foreach (Actor entity in map.Entities.Items) {
                MapConsole.Children.Add(entity);
            }

            // Subscribe to the Entities ItemAdded listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemAdded += OnMapEntityAdded;

            // Subscribe to the Entities ItemRemoved listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemRemoved += OnMapEntityRemoved;
        }


        // Remove an Entity from the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Actor> args) {
            MapConsole.Children.Remove(args.Item);
        }

        // Add an Entity to the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Actor> args) {
            MapConsole.Children.Add(args.Item);
        }


        
    }
}