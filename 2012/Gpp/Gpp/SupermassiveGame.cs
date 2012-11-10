using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Planet _mainPlanet;

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
            _background = Content.Load<Texture2D>("star_field_1");
            var playerOneTexture = Content.Load<Texture2D>("bloop-sprite");
            var playerTwoTexture = Content.Load<Texture2D>("bloop-sprite");

            ProjectileTexture = Content.Load<Texture2D>("projectile");
            GameObjects = new List<GameObject>();
            var centerScreen = new Vector2(width / 2, height / 2);
            var mainPlanetHeight = height * 0.3f;
            _mainPlanet = AddPlanet("Green-Planet", 0.5f, centerScreen, height, 5E13f, 0.47f);
            GameObjects.Add(_mainPlanet);
            //AddPlanet("Rock-Planet-Flat", 0.3f, centerScreen, height, 5E13f, 0.8f);

            // add planets randomly
            //var rand = new Random();
            //for (int i = 0; i < 4; i++)
            //{
            //    Planet planet;
            //    do
            //    {
            //        var origin = new Vector2(rand.Next(width), rand.Next(height));
            //        planet = AddPlanet("Rock-Planet-Flat", 0.15f, origin, height, 1E13f, 0.8f);
            //    }
            //    while(GameObjects.OfType<Planet>().Any(p => p.BoundingSphere.Transform(Matrix.CreateScale(2.0f)).Intersects(planet.BoundingSphere)));
            //    //;
            //    GameObjects.Add(planet);
            //}
            GameObjects.Add(AddPlanet("Green-Planet", 0.25f, new Vector2(width * 0.1f, height * 0.1f), height, 1E13f, 0.5f));
            GameObjects.Add(AddPlanet("Green-Planet", 0.25f, new Vector2(width * 0.8f, height * 0.2f), height, 1E13f, 0.5f));
            GameObjects.Add(AddPlanet("Green-Planet", 0.25f, new Vector2(width * 0.3f, height * 0.7f), height, 1E13f, 0.5f));
            GameObjects.Add(AddPlanet("Green-Planet", 0.25f, new Vector2(width * 0.9f, height * 0.6f), height, 1E13f, 0.5f));


            var playerRadius =  (mainPlanetHeight / 2.0f) + 10f;
            _players = new List<Player> { 
                new Player(this, PlayerIndex.One, playerOneTexture, playerRadius, new Vector2(-1, 0), centerScreen), 
                new Player(this, PlayerIndex.Two, playerTwoTexture, playerRadius, new Vector2(1, 0), centerScreen)
            };
            GameObjects.AddRange(_players);

            //GameObjects.Add(new MovableObject(this, _background, new Vector2(800, 500), 0.1f, 500000000000000));
            //GameObjects.Add(new MovableObject(this, _background, new Vector2(800, 1000), 0.1f, 500000000000000));
            //GameObjects.Add(new MovableObject(this, _background, new Vector2(200, 750), 0.05f, 300000));
        }

        private Planet AddPlanet(string textureName, float scale, Vector2 origin, int displayHeight, float mass, float boundingSpherePercent)
        {
            var planetTexture = Content.Load<Texture2D>(textureName);
            var planetHeight = (displayHeight * scale);
            var displayScale = planetHeight / planetTexture.Height;

            var planet = new Planet(this, planetTexture, origin, displayScale, mass, boundingSpherePercent);
            return planet;
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

            var updateGameObjects = new List<GameObject>(GameObjects);
            foreach (var gameObject in updateGameObjects)
                gameObject.Update(gameTime.ElapsedGameTime);

            foreach (var projectile in GameObjects.OfType<Projectile>().ToList())
            {
                foreach (var gameObject in GameObjects.Where(g => !(g is Projectile)).ToList())
                {
                    if (projectile.BoundingSphere.Intersects(gameObject.BoundingSphere))
                    {
                        var player = gameObject as Player;
                        if (player != null) player.TakeDamage(projectile);

                        var planet = gameObject as Planet;
                        if (planet != null && planet != _mainPlanet) planet.TakeDamage(projectile);
                        GameObjects.Remove(projectile);
                    }
                }
            }

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
            _spriteBatch.Draw(_background, new Rectangle(0, 0, 1920, 1080), Color.White);
            foreach (var gameObject in GameObjects)
                gameObject.Draw(_spriteBatch, gameTime.ElapsedGameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
