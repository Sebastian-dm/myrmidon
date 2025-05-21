using myrmidon.Entities;
using myrmidon.UI;
using myrmidon;
using SadConsole;
using SadConsole.Controls;
using SadRogue.Primitives;
using System;

public class UIManager : ContainerConsole {
    public CellSurface MapSurface;
    public ScrollingConsole MapView;
    public Window MapWindow;
    public MessageLogWindow MessageLog;
    public SadConsole.Themes.Colors CustomColors;
    public bool WaitingForInput = true;

    public UIManager() {
        IsVisible = true;
        IsFocused = true;

        Parent = Game.Instance.Screen;  // Updated for SadConsole 10+
    }

    public void Init() {
        SetupCustomColors();
        CreateMapSurface();
        MessageLog = new MessageLogWindow(GameLoop.GameWidth, GameLoop.GameHeight / 4, "Message Log");
        Children.Add(MessageLog);
        MessageLog.Show();
        MessageLog.Position = new Point(0, GameLoop.GameHeight * 3 / 4);
        MessageLog.Add("Started game from path: " + System.IO.Directory.GetCurrentDirectory());
        LoadMap(GameLoop.World.CurrentMap);
        CreateMapWindow(GameLoop.GameWidth, GameLoop.GameHeight * 3 / 4, "Game Map");
        UseMouse = true;
    }

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

    public void CreateMapSurface() {
        MapSurface = new CellSurface(GameLoop.GameWidth, GameLoop.GameHeight);
    }

    private void LoadMap(Maps.Map map) {
        MapSurface = new CellSurface(map.Width, map.Height, SadConsole.Game.Instance.Fonts.DefaultFont);

        // Load tiles into MapSurface.Cells as needed from your map data
        for (int y = 0; y < map.Height; y++) {
            for (int x = 0; x < map.Width; x++) {
                var tile = map.Tiles[x, y];
                MapSurface.SetGlyph(x, y, tile.Glyph);
                MapSurface.SetForeground(x, y, tile.Foreground);
                MapSurface.SetBackground(x, y, tile.Background);
            }
        }

        // Create/Reset SurfaceView after loading the surface
        MapView = new SurfaceViewer(MapSurface) {
            ViewArea = new Rectangle(0, 0, GameLoop.GameWidth - 2, GameLoop.GameHeight * 3 / 4 - 2), // Fit window size later
            Position = new Point(1, 1)
        };

        SyncMapEntities(map);
    }

    public void RefreshConsole() {
        MapSurface.IsDirty = true;
    }

    public void CenterOnActor(Actor actor) {
        var viewArea = MapView.ViewArea;
        var viewX = actor.Position.X - viewArea.Width / 2;
        var viewY = actor.Position.Y - viewArea.Height / 2;

        viewX = Math.Clamp(viewX, 0, Math.Max(0, MapSurface.Width - viewArea.Width));
        viewY = Math.Clamp(viewY, 0, Math.Max(0, MapSurface.Height - viewArea.Height));

        MapView.ViewArea = new Rectangle(viewX, viewY, viewArea.Width, viewArea.Height);
    }

    public override void Update(TimeSpan timeElapsed) {
        base.Update(timeElapsed);
    }

    public void CreateMapWindow(int width, int height, string title) {
        MapWindow = new Window(width, height) {
            CanDrag = true,
            Title = title.Align(HorizontalAlignment.Center, width - 2)
        };

        // Adjust ViewArea size for window borders
        MapView.ViewArea = new Rectangle(0, 0, width - 2, height - 2);
        MapView.Position = new Point(1, 1);

        // Add close button (optional)
        var closeButton = new Button(3, 1) {
            Position = new Point(0, 0),
            Text = "[X]"
        };
        MapWindow.Add(closeButton);

        MapWindow.Children.Add(MapView);
        Children.Add(MapWindow);
        MapWindow.Show();
    }

    private void SyncMapEntities(Maps.Map map) {
        MapView.Children.Clear();

        foreach (Actor entity in map.Entities.Items) {
            MapView.Children.Add(entity);
        }

        map.Entities.ItemAdded += OnMapEntityAdded;
        map.Entities.ItemRemoved += OnMapEntityRemoved;
    }

    public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Actor> args) {
        MapView.Children.Remove(args.Item);
    }

    public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Actor> args) {
        MapView.Children.Add(args.Item);
    }
}
