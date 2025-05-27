
using Myrmidon.Simulation;
using Myrmidon.Core.Actors;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;

using Myrmidon.Core.GameState;
using Myrmidon.Core.UI;
using Myrmidon.Core.Simulation;
using Myrmidon.Core.Rules;

namespace Myrmidon.App {
    public class MainGame {
        //private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;

        //// World & Simulation
        //private IGameContext _context;
        //private WorldController _worldController;
        //private ActionController _actionController;
        ////private TurnManager _turnManager;

        //// Rendering
        //private TileRenderer _tileRenderer;
        //private Texture2D _tileset;
        //private SpriteFont _font;
        //private Rectangle _viewArea;

        //// Input
        ////private IInputHandler _inputHandler;

        //public MainGame() {
        //    _graphics = new GraphicsDeviceManager(this) {
        //        PreferredBackBufferWidth = 1280,
        //        PreferredBackBufferHeight = 720
        //    };

        //    Content.RootDirectory = "Content";
        //    IsMouseVisible = true;
        //}

        //protected override void Initialize() {
        //    base.Initialize();

        //    //Initialize core systems
        //    var fovSystem = new FovSystem();      // Field of View system
        //    var uiService = new UIService();      // User Interface service

        //    // Initialize game state
        //    var world = new World(90, 60); // Holds game state and entities
        //    _context = new GameContext(world); // Holds game state and context

        //    // Initialize controllers
        //    _worldController = new WorldController(_context, fovSystem, uiService); // Handles game state and logic
        //    _actionController = new ActionController(_context, fovSystem, uiService); // Handles actions and commands

        //    //_turnManager = new TurnManager();   // Handles turn-based action resolution
        //    //_inputHandler = new MonoGameInputHandler(); // From Game.Input layer

        //    // Camera/View area
        //    _viewArea = new Rectangle(0, 0, 80, 45);  // In tiles
        //}

        //protected override void LoadContent() {
        //    _spriteBatch = new SpriteBatch(GraphicsDevice);
        //    _tileset = Content.Load<Texture2D>("tiles");
        //    _font = Content.Load<SpriteFont>("font");

        //    _tileRenderer = new TileRenderer(_spriteBatch, _tileset, 16, _font);
        //}

        //protected override void Update(GameTime gameTime) {
        //    // Exit shortcut
        //    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        //        Exit();

        //    // Update game state
        //    _worldController.Update();
        //    _actionController.Update();

        //    // Process Input
        //    //var command = _inputHandler.GetCommand(); // e.g., Move(North)
        //    //if (command != null) {
        //    //    _world.ExecuteCommand(command); // Core logic handles effects
        //    //    _turnManager.AdvanceTurn(_world); // Progress world state
        //    //}

        //    base.Update(gameTime);
        //}

        //protected override void Draw(GameTime gameTime) {
        //    GraphicsDevice.Clear(Color.Black);

        //    _spriteBatch.Begin();

        //    _tileRenderer.RenderMap(_context.World.CurrentMap, _viewArea);
        //    _tileRenderer.RenderEntities(_context.World.Entities.Items, _viewArea);

        //    _spriteBatch.End();

        //    base.Draw(gameTime);
        //}
    }
}
