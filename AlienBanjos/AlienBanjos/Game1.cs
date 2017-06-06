using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AlienBanjos
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Class calling
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        cButton btnPlay;
        cButton btnLoad;
        Scrolling scrolling1, scrolling2;
        SoundEffect soundLaser;
        Song song;
        public Texture2D healthTexture;
        public Rectangle healthRectangle;
        bool loadAvailable = false;

        public SpriteFont Font { get; set; }


        // Variable and List declaration
        int screenWidth, screenHeight;
        public bool GameOver = false;
        public bool Firing = false;

        public int HighScore { get; set; }

        private AlienBanjos.GameState state;
        public Textures Textures { get; set; }


        // Game State
        public enum GameState
        {
            MainMenu,
            Playing,
            GameOver
        };

        GameState CurrentGameState = GameState.MainMenu;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
        }


        protected override void Initialize()
        {
            base.Initialize();

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);


            // Sound Effects and Muisc
            soundLaser = Content.Load<SoundEffect>("LaserShot");
            song = Content.Load<Song>("SpaceMusic");


            loadAvailable = AlienBanjos.GameState.HasExistingGame();

            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            // Health
            healthTexture = Content.Load<Texture2D>("Health");

            Font = Content.Load<SpriteFont>("Arial");

            // Screen Parameters         
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;


            Textures = new Textures();
            Textures.Load(Content);


            //Background
            scrolling1 = new Scrolling(Textures.Background1, new Rectangle(0, 0, 800, 800));
            scrolling2 = new Scrolling(Textures.Background2, new Rectangle(0, 800, 800, 800));

            //Main Menu
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;


            // Main Menu Button
            btnPlay = new cButton(Content.Load<Texture2D>("Menu/Button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350, 300));

            btnLoad = new cButton(Content.Load<Texture2D>("Menu/load"), graphics.GraphicsDevice);
            btnLoad.setPosition(new Vector2(350, 335));


            // start a background demo game

            state = new AlienBanjos.GameState();
            state.NewGame(screenWidth, screenHeight, Textures);

        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Gamestate
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    IsMouseVisible = true;

                    if (btnPlay.isClicked == true)
                    {
                        btnPlay.isClicked = false;
                        CurrentGameState = GameState.Playing;

                        // create a new game
                        state = new AlienBanjos.GameState();
                        state.NewGame(screenWidth, screenHeight, Textures);
                    }

                    if (btnLoad.isClicked == true)
                    {
                        btnLoad.isClicked = false;

                        CurrentGameState = GameState.Playing;

                        // create a new game
                        state = AlienBanjos.GameState.LoadGame(screenWidth, screenHeight, Textures);

                    }

                    btnPlay.Update(mouse);

                    if (loadAvailable)
                    {
                        btnLoad.Update(mouse);
                    }

                    UpdateGame(dt, null);

                    break;

                case GameState.Playing:

                    UpdateGame(dt, keyboard);
                    IsMouseVisible = false;

                    break;
            }



            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }


            base.Update(gameTime);
        }



        public void UpdateGame(float dt, KeyboardState? keyboard)
        {
            state.TotalGameTime += dt;

            // check whether previous update killed the player

            if (state.Player.Health <= 0)
            {
                CurrentGameState = GameState.MainMenu;
                if (state.Player.Score > HighScore)
                {
                    HighScore = state.Player.Score;
                }
            }


            // Game Controls     

            if (keyboard.HasValue)
            {
                if (keyboard.Value.IsKeyDown(Keys.Right) || keyboard.Value.IsKeyDown(Keys.D))
                {
                    state.Player.Direction = Direction.Right;
                }
                else if (keyboard.Value.IsKeyDown(Keys.Left) || keyboard.Value.IsKeyDown(Keys.A))
                {
                    state.Player.Direction = Direction.Left;
                }
                else
                {
                    state.Player.Direction = null;
                }

                if (keyboard.Value.IsKeyDown(Keys.Space))
                {
                    if (state.FireLaser(screenWidth, screenHeight))
                    {
                        soundLaser.Play();
                    }
                }

                if (keyboard.Value.IsKeyDown(Keys.S))
                {
                    state.SaveGame();
                }
            }


            // create more enemys

            state.CreateBanjos(screenWidth, screenHeight);


            //Scrolling Background

            if (scrolling1.rectangle.Y + scrolling1.texture.Height <= 0)
                scrolling1.rectangle.Y = scrolling2.rectangle.Y + scrolling2.texture.Height;

            if (scrolling2.rectangle.Y + scrolling2.texture.Height <= 0)
                scrolling2.rectangle.Y = scrolling1.rectangle.Y + scrolling1.texture.Height;

            scrolling1.Update();
            scrolling2.Update();


            // Player updates

            state.Player.Update(dt);
            healthRectangle = new Rectangle(20, 60, state.Player.Health, 20);


            // Laser Updates

            foreach (Laser laser in state.Lasers)
            {
                laser.Update(dt);
            }


            //Enemy Updates

            foreach (PlainBanjo plainBanjo in state.PlainBanjos)
            {
                if (plainBanjo.IsDead != true)
                {
                    plainBanjo.Update(dt, state.Lasers);
                }
            }

            foreach (HunterBanjo hunterBanjo in state.HunterBanjos)
            {
                if (hunterBanjo.IsDead != true)
                {
                    hunterBanjo.Update(dt, state.Lasers);
                }
            }

            foreach (DeadlyBanjo deadlyEnemy in state.DeadlyBanjos)
            {
                if (deadlyEnemy.IsDead != true)
                {
                    deadlyEnemy.Update(dt, state.Lasers);
                }
            }
        }


        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Game State Drawing
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    //spriteBatch.Draw(Content.Load<Texture2D>("Menu/MainMenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                                        
                    DrawGame();


                    spriteBatch.Draw(Content.Load<Texture2D>("Background/scroll"), new Rectangle(280, 200, 234, 300), Color.White);

                    
                    btnPlay.Draw(spriteBatch);


                    if (loadAvailable)
                    {
                        btnLoad.Draw(spriteBatch);
                    }

                    

                    break;
                case GameState.Playing:

                    DrawGame();

                    break;


                case GameState.GameOver:
                    
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }


        public void DrawGame()
        {

            // Draw Background
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);


            spriteBatch.Draw(Textures.Player, state.Player.Position, Color.White);


            foreach (var enemy in state.PlainBanjos)
            {
                if (!enemy.IsDead)
                    spriteBatch.Draw(Textures.PlainBanjo, enemy.Position, Color.White);
            }

            foreach (var enemy in state.HunterBanjos)
            {
                if (!enemy.IsDead)
                    spriteBatch.Draw(Textures.HunterBanjo, enemy.Position, Color.White);
            }


            foreach (var enemy in state.DeadlyBanjos)
            {
                if (!enemy.IsDead)
                    spriteBatch.Draw(Textures.DeadlyBanjo, enemy.Position, Color.White);
            }


            foreach (var laser in state.Lasers)
            {
                if (!laser.IsDead)
                    spriteBatch.Draw(Textures.Laser, laser.Position, Color.White);
            }


            // Draw Health
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);


            spriteBatch.DrawString(Font, "High Score: " + HighScore, new Vector2(20, 10), Color.White);
            spriteBatch.DrawString(Font, "Score: " + state.Player.Score.ToString(), new Vector2(20, 30), Color.White);
        }

        //// Manage Explosions
        //public void ManageExplosions()
        //{
        //    for (int i = 0; i < explosionList.Count; i++)
        //    {
        //        if (!explosionList[i].isVisible)
        //        {
        //            explosionList.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //}

        
    }
}
