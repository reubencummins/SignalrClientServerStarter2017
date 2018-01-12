using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprites;
using Microsoft.Xna.Framework.Audio;
using CameraNS;
using GameData;

namespace MonoGameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        HubConnection serverConnection;
        IHubProxy proxy;
        Vector2 worldCoords;
        SpriteFont messageFont;
        Texture2D backGround;
        private string connectionMessage;
        private bool Connected;
        private Rectangle worldRect;
        private bool Joined;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // Azure endpoint http://ppgameserver.azurewebsites.net/
            serverConnection = new HubConnection("http://localhost:54048/");
            //serverConnection = new HubConnection("http://ppgameserver.azurewebsites.net/");
            serverConnection.StateChanged += ServerConnection_StateChanged;
            proxy = serverConnection.CreateHubProxy("GameHub");
            connectionMessage = string.Empty;
            serverConnection.Start();
            base.Initialize();
        }

        private void ServerConnection_StateChanged(StateChange State)
        {
            switch (State.NewState)
            {
                case ConnectionState.Connected:
                    connectionMessage = "Connected......";
                    Connected = true;
                    startGame();
                    break;
                case ConnectionState.Disconnected:
                    connectionMessage = "Disconnected.....";
                    if (State.OldState == ConnectionState.Connected)
                        connectionMessage = "Lost Connection....";
                    Connected = false;
                    break;
                case ConnectionState.Connecting:
                    connectionMessage = "Connecting.....";
                    Connected = false;
                    break;
            }
        }

        private void startGame()
        {
            Action<int, int> joined = cJoined;
            proxy.On("joined", joined);
            proxy.Invoke("join");
            Action<string, Position> moved = opponentMoved;
            

            Services.AddService(proxy);
                                            
        }

        private void opponentMoved(string opponentID, Position opponentPosition)
        {
            Opponent op = Components.
        }

        private void cJoined(int arg1, int arg2)
        {
            worldCoords = new Vector2(arg1, arg2);
            // Setup Camera
            worldRect = new Rectangle(new Point(0, 0), worldCoords.ToPoint());
            new Camera(this, Vector2.Zero, worldCoords);

            Joined = true;
            // Setup Player
            new Player(this, new Texture2D[] {
                            Content.Load<Texture2D>(@"Textures\left"),
                            Content.Load<Texture2D>(@"Textures\right"),
                            Content.Load<Texture2D>(@"Textures\up"),
                            Content.Load<Texture2D>(@"Textures\down"),
                            Content.Load<Texture2D>(@"Textures\stand"),
                        }, new SoundEffect[] { }, GraphicsDevice.Viewport.Bounds.Center.ToVector2(),
                        8, 0, 5.0f);


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            messageFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");
            Services.AddService<SpriteFont>(Content.Load<SpriteFont>(@"Fonts\PlayerFont"));
            backGround = Content.Load<Texture2D>(@"Textures\background");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (!Connected && !Joined) return;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (Connected && Joined)
            {
                DrawPlay();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(messageFont, connectionMessage,
                                new Vector2(20, 20), Color.White);
                spriteBatch.End();
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawPlay()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.CurrentCameraTranslation);
            spriteBatch.Draw(backGround, worldRect, Color.White);
            spriteBatch.End();
        }
    }
}
