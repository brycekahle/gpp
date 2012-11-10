using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Gpp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SupermassiveGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;        
        Texture2D _background;
        public Texture2D ProjectileTexture { get; set; }

        public List<GameObject> GameObjects { get; private set; }
        List<Player> _players;

        public SupermassiveGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            var width = GraphicsDevice.Viewport.Width;
            var height = GraphicsDevice.Viewport.Height;

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("background");
            var playerTexture = Content.Load<Texture2D>("player");

            ProjectileTexture = Content.Load<Texture2D>("background");
            GameObjects = new List<GameObject>();            

            var planetTexture = Content.Load<Texture2D>("Rock-Planet-Flat");
            var planetHeight = (height * 0.3f);
            var scale = planetHeight / planetTexture.Height;
            var centerScreen = new Vector2(width / 2, height / 2);
            GameObjects.Add(new Planet(this, planetTexture, centerScreen, scale, 5E10f));

            _players = new List<Player> { 
                new Player(this, PlayerIndex.One, playerTexture, centerScreen + new Vector2(-planetHeight/2, 0), new Vector2(-1, 0)), 
                new Player(this, PlayerIndex.Two, playerTexture, centerScreen + new Vector2(planetHeight/2, 0), new Vector2(1, 0))
            };
            GameObjects.AddRange(_players);

            //GameObjects.Add(new MovableObject(this, _background, new Vector2(800, 500), 0.1f, 500000000000000));
            //GameObjects.Add(new MovableObject(this, _background, new Vector2(800, 1000), 0.1f, 500000000000000));
            //GameObjects.Add(new MovableObject(this, _background, new Vector2(200, 750), 0.05f, 300000));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (var gameObject in GameObjects)
                gameObject.Update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();
            //_spriteBatch.Draw(_background, new Rectangle(0, 0, 1920, 1080), Color.White);
            foreach (var gameObject in GameObjects)
                gameObject.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
