using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Linq;
using System.Collections.Generic;

namespace Game1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random RandomInt = new Random();
        Texture2D appleTexture;
        Texture2D tileTexture;
        Texture2D snakeTexture;
        Texture2D snakeTile;
        Texture2D loseScreen;
        SnakePrint SnakePrint;
        SpriteFont font;
        Vector2 applePlace;
        Vector2[] snakeArray = new Vector2[3];
        int gameSize;
        int movement = 0; // 0-right, 1-up, 2-left, 3-down
        int newMovement = 0;
        int gamePlay = 1;
        int appleCount = 0;
        int tileTextureWidth = 64;
        int tileTextureHeight = 64;
        int[,] gameMapArray;
        float timeFrame;
        float deltaTime;
        float gameScale;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(50);
        }

        protected override void Initialize()
        {
            this.Window.Position = new Point(0, 0);

            gameSize = 15;
            gameScale = 1f;
            gameMapArray = new int[gameSize, gameSize];
            snakeArray = new Vector2[] { new Vector2(5, 5), new Vector2(4, 5), new Vector2(3, 5) };
            applePlace = new Vector2(2, 3);
            deltaTime = 350f;
            timeFrame = 0f;

            graphics.PreferredBackBufferWidth = tileTextureWidth * gameSize;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = tileTextureHeight * gameSize;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileTexture = Content.Load<Texture2D>("tile");
            appleTexture = Content.Load<Texture2D>("apple");
            snakeTexture = Content.Load<Texture2D>("snake");
            snakeTile = Content.Load<Texture2D>("snakeTile");
            loseScreen = Content.Load<Texture2D>("loseScreen");
            font = Content.Load<SpriteFont>("Font");
            SnakePrint = new SnakePrint(snakeTile, 64, 64, 1f);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && movement != 2)
            {
                newMovement = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && movement != 3)
            {
                newMovement = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && movement != 0)
            {
                newMovement = 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && movement != 1)
            {
                newMovement = 3;
            }

            timeFrame = timeFrame + (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeFrame > deltaTime && gamePlay == 1)
            {
                movement = newMovement;
                timeFrame = 0f;
                Vector2 newHeadPos = snakeArray[0];

                if (movement == 0) // right
                { 
                    newHeadPos.X = newHeadPos.X + 1;
                    if (newHeadPos.X > gameSize - 1) newHeadPos.X = 0;
                }
                else if (movement == 1) // up
                {
                    newHeadPos.Y = newHeadPos.Y - 1;
                    if (newHeadPos.Y < 0) newHeadPos.Y = gameSize - 1;
                }
                else if (movement == 2) // left
                { 
                    newHeadPos.X = newHeadPos.X - 1;
                    if (newHeadPos.X < 0) newHeadPos.X = gameSize - 1;
                }
                else if (movement == 3) // down
                {
                    newHeadPos.Y = newHeadPos.Y + 1;
                    if (newHeadPos.Y > gameSize - 1) newHeadPos.Y = 0;
                }

                for (int i = 0; i < snakeArray.Length; i++)
                {
                    if (snakeArray[i] == newHeadPos)
                    {
                        gamePlay = 0;
                    }
                }

                int eatenApple = 0;
                List<Vector2> newArray = snakeArray.ToList();
                newArray.Insert(0, newHeadPos);
                if (newHeadPos == applePlace)
                {
                    eatenApple = 1;
                }
                else
                {
                    newArray.RemoveAt(newArray.Count - 1);
                }
                snakeArray = newArray.ToArray();
                
                if (eatenApple == 1)
                {
                    // change Apple Position after eat
                    int randomLength = gameSize * gameSize - 1 - snakeArray.Length;
                    int newPosIndex = RandomInt.Next(0, randomLength);
                    int curPosition = 0;

                    for (int i = 0; i < snakeArray.Length - 1; i++)
                    {
                        gameMapArray[(int)snakeArray[i].X, (int)snakeArray[i].Y] = 1;
                    }

                    for (int i = 0; i < gameSize; i++)
                    {
                        for (int j = 0; j < gameSize; j++)
                        {
                            if (gameMapArray[i, j] == 0) {
                                curPosition++;
                            }
                            if (curPosition == newPosIndex)
                            {
                                applePlace.X = i;
                                applePlace.Y = j;
                                break;
                            }
                        }
                        if (curPosition == newPosIndex) break;
                    }

                    for (int i = 0; i < snakeArray.Length - 1; i++)
                    {
                        gameMapArray[(int)snakeArray[i].X, (int)snakeArray[i].Y] = 0;
                    }

                    appleCount++;
                    deltaTime = deltaTime * 0.99f;
                }

                Window.Title = appleCount.ToString();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (gamePlay == 1)
            {
                Vector2 placementVector = new Vector2(0, 0);

                //print background
                for (int i = 0; i < gameSize; i++)
                {
                    for (int j = 0; j < gameSize; j++)
                    {
                        placementVector.X = i * tileTextureWidth;
                        placementVector.Y = j * tileTextureHeight;
                        spriteBatch.Draw(
                            tileTexture, 
                            placementVector, 
                            new Rectangle(0, 0, tileTexture.Width, tileTexture.Height), 
                            Color.White, 
                            0f, 
                            Vector2.Zero,
                            (float)tileTextureWidth / (float)tileTexture.Width,
                            SpriteEffects.None, 0f);
                        
                    }
                }

                //print apple
                placementVector.X = applePlace.X * tileTextureHeight;
                placementVector.Y = applePlace.Y * tileTextureHeight;
                SnakePrint.Draw(placementVector, 4, 1, spriteBatch);

                //print snake
                int what = 0;
                int type = 0;
                for (int k = 0; k < snakeArray.Length; k++)
                {
                    if (k == 0)
                    {
                        what = 2;
                        int[] typeArray = { 2, 3, 4, 1 };
                        if (Math.Abs(snakeArray[k].X - snakeArray[k + 1].X) > 1 || Math.Abs(snakeArray[k].Y - snakeArray[k + 1].Y) > 1)
                        {
                            typeArray = new int[] { 3, 2, 1, 4 };
                        }

                        if (snakeArray[k].X > snakeArray[k + 1].X)
                        {
                            type = typeArray[0];
                        }
                        else if (snakeArray[k].X < snakeArray[k + 1].X)
                        {
                            type = typeArray[1];
                        }
                        else if (snakeArray[k].Y > snakeArray[k + 1].Y)
                        {
                            type = typeArray[2];
                        }
                        else if (snakeArray[k].Y < snakeArray[k + 1].Y)
                        {
                            type = typeArray[3];
                        }
                    }
                    else if (k == snakeArray.Length - 1)
                    {
                        what = 3;
                        int[] typeArray = { 3, 2, 1, 4 };
                        if (Math.Abs(snakeArray[k].X - snakeArray[k - 1].X) > 1 || Math.Abs(snakeArray[k].Y - snakeArray[k - 1].Y) > 1)
                        {
                            typeArray = new int[] { 2, 3, 4, 1 };
                        }

                        if (snakeArray[k].X > snakeArray[k - 1].X)
                        {
                            type = typeArray[0];
                        }
                        else if (snakeArray[k].X < snakeArray[k - 1].X)
                        {
                            type = typeArray[1];
                        }
                        else if (snakeArray[k].Y > snakeArray[k - 1].Y)
                        {
                            type = typeArray[2];
                        }
                        else if (snakeArray[k].Y < snakeArray[k - 1].Y)
                        {
                            type = typeArray[3];
                        }
                    }
                    else
                    {
                        what = 1;
                        if (snakeArray[k].X != snakeArray[k - 1].X && snakeArray[k].X != snakeArray[k + 1].X)
                        {
                            type = 3;
                        }
                        else if (snakeArray[k].Y != snakeArray[k - 1].Y && snakeArray[k].Y != snakeArray[k + 1].Y)
                        {
                            type = 5;
                        }
                        else if (snakeArray[k].X > snakeArray[k - 1].X && snakeArray[k].Y < snakeArray[k + 1].Y || snakeArray[k].X > snakeArray[k + 1].X && snakeArray[k].Y < snakeArray[k - 1].Y)
                        {
                            type = 4;
                        }
                        else if (snakeArray[k].X < snakeArray[k - 1].X && snakeArray[k].Y > snakeArray[k + 1].Y || snakeArray[k].X < snakeArray[k + 1].X && snakeArray[k].Y > snakeArray[k - 1].Y)
                        {
                            type = 1;
                        }
                        else if (snakeArray[k].X < snakeArray[k - 1].X && snakeArray[k].Y < snakeArray[k + 1].Y || snakeArray[k].X < snakeArray[k + 1].X && snakeArray[k].Y < snakeArray[k - 1].Y)
                        {
                            type = 2;
                        }
                        else if (snakeArray[k].X > snakeArray[k - 1].X && snakeArray[k].Y > snakeArray[k + 1].Y || snakeArray[k].X > snakeArray[k + 1].X && snakeArray[k].Y > snakeArray[k - 1].Y)
                        {
                            type = 6;
                        }
                    }
                    placementVector.X = snakeArray[k].X * tileTextureWidth;
                    placementVector.Y = snakeArray[k].Y * tileTextureWidth;
                    SnakePrint.Draw(placementVector, what, type, spriteBatch);
                }
            }
            else {
                spriteBatch.Draw(loseScreen, Vector2.Zero, new Rectangle(0, 0, loseScreen.Width, loseScreen.Height), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
