using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PoliTetris
{
    public class LevelComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private Effect effect;
        private GameBoard gameBoard;
        private Texture2D background;
        private BlockElement blockTile;
        private Vector2 gameBoardPosition;
        private Texture2D[] tileSprites;
        private BlockGenerator blockGenerator;
        private float speed;
        private float timeSinceLastFall;
        private float timeSinceLastKeyPress;
        private float keyDelay;
        private BlockElement nextBlock;
        private SpriteFont font;
        private SoundEffect rowSound;
        private Texture2D sprPause;

        public LevelComponent(Game1 game) : base(game)
        {
            this.game = game;
        }

        public enum eGameState
        {
            Playing,
            Pause,
        }
        public eGameState gameState;

        public override void Initialize()
        {
            int[,] pattern = new int[4, 4];
            pattern[1, 0] = 1;
            pattern[2, 0] = 1;
            pattern[0, 1] = 1;
            pattern[1, 1] = 1;
            blockTile = new BlockElement(pattern);
            speed = 1;
            timeSinceLastFall = 0;
            keyDelay = 0.15f;
            timeSinceLastKeyPress = 0;
            gameBoardPosition = new Vector2(366, 50);
            blockGenerator = new BlockGenerator("pattern.dll");
            blockGenerator.LoadBlocks();
            gameBoard = new GameBoard(10, 20, gameBoardPosition);
            nextBlock = blockGenerator.Generate(7);
            NextBlock();
            game.player = new Player();
            gameState = eGameState.Playing;
            base.Initialize();
        }

        public void NextBlock()
        {
            blockTile = nextBlock;
            blockTile.position = gameBoard.InsertBlock(blockTile);
            nextBlock = blockGenerator.Generate(7);
        }





        protected override void LoadContent()
        {
            tileSprites = new Texture2D[15];
            for (int i = 0; i < 15; i++)
            {
                tileSprites[i] = game.Content.Load<Texture2D>("15");
            }
            background = game.Content.Load<Texture2D>("tlo_tetris");
            effect = game.Content.Load<Effect>("effect");
            font = game.Content.Load<SpriteFont>("font_blox");
            rowSound = game.Content.Load<SoundEffect>("alarm");
            sprPause = game.Content.Load<Texture2D>("pause");
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            if (gameState == eGameState.Playing)
            {

                if (game.NewKey(Keys.Enter) || game.NewKey(Keys.Up))
                {
                    blockTile.Rotate();
                    if (gameBoard.Collision(blockTile, blockTile.position))
                        for (int i = 0; i < 3; i++)
                            blockTile.Rotate();
                }

                float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

                float timeBetweenFalls = speed;
                if (game.keyboardState.IsKeyDown(Keys.RightControl) && (speed > 0.15f))
                    timeBetweenFalls = 0.15f;

                if (game.NewKey(Keys.Down))
                {
                    while (!gameBoard.Collision(blockTile, new Vector2(blockTile.position.X, blockTile.position.Y + 1)))
                        blockTile.Fall();
                    timeSinceLastFall += timeBetweenFalls;
                }

                if ((game.keyboardState.IsKeyDown(Keys.Right)) && (!gameBoard.Collision(blockTile, new Vector2(blockTile.position.X + 1, blockTile.position.Y))))
                    blockTile.position.X++;
                if ((game.keyboardState.IsKeyDown(Keys.Left)) && (!gameBoard.Collision(blockTile, new Vector2(blockTile.position.X - 1, blockTile.position.Y))))
                    blockTile.position.X--;
                timeSinceLastKeyPress += seconds;

                if (timeSinceLastKeyPress > keyDelay)
                {
                    if ((game.keyboardState.IsKeyDown(Keys.Right)) &&
                       (!gameBoard.Collision(blockTile, new Vector2(blockTile.position.X + 1, blockTile.position.Y))))
                        blockTile.position.X++;
                    if ((game.keyboardState.IsKeyDown(Keys.Left)) &&
                        (!gameBoard.Collision(blockTile, new Vector2(blockTile.position.X - 1, blockTile.position.Y))))
                        blockTile.position.X--;
                    timeSinceLastKeyPress = 0;
                }

                timeSinceLastFall += seconds;
                if (timeSinceLastFall >= timeBetweenFalls)
                {
                    blockTile.Fall();
                    if (gameBoard.Collision(blockTile, blockTile.position))
                    {
                        blockTile.position.Y--;
                        gameBoard.Merge(blockTile);
                        int rows = gameBoard.RemoveRows();

                        if (rows > 0)
                            rowSound.Play();

                        game.player.rows += rows;

                        switch (rows)
                        {
                            case 1: game.player.score += game.player.level * 40 + 40; break;
                            case 2: game.player.score += game.player.level * 100 + 100; break;
                            case 3: game.player.score += game.player.level * 300 + 300; break;
                            case 4: game.player.score += game.player.level * 1200 + 1200; break;
                        }
                        int level = game.player.rows / 10 + 1;
                        if (game.player.level != level)
                        {
                            game.player.level = level;
                            speed = speed * 5 / 6;
                        }

                        NextBlock();

                       if (gameBoard.Collision(blockTile, blockTile.position))
                            game.Exit();
                    }
                    timeSinceLastFall = 0;
                }

                if (game.keyboardState.IsKeyDown(Keys.P))
                {
                    MediaPlayer.Pause();
                    gameState = eGameState.Pause;
                }

            }


            if (gameState == eGameState.Pause)
            {
                if (game.keyboardState.IsKeyDown(Keys.Y))
                   game.Exit();             

                if (game.keyboardState.IsKeyDown(Keys.N))
                {
                    gameState = eGameState.Playing; 
                    MediaPlayer.Resume();
                }
            }

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            game._spriteBatch.Begin();
            game._spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            game._spriteBatch.End();

            game._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.CurrentTechnique.Passes[0].Apply();
            blockTile.Draw(gameBoardPosition, game._spriteBatch, tileSprites);
            game._spriteBatch.End();

            game._spriteBatch.Begin();
            gameBoard.Draw(game._spriteBatch, tileSprites);
            nextBlock.Draw(new Vector2(930, 200), game._spriteBatch, tileSprites);
            game._spriteBatch.DrawString(font, "Punkty\n " + game.player.score.ToString(), new Vector2(30, 390), Color.Red);
            game._spriteBatch.DrawString(font, "Poziom\n " + game.player.level.ToString(), new Vector2(215, 390), Color.Red);

            if (gameState == eGameState.Pause)
            {
                game._spriteBatch.Draw(sprPause, new Rectangle(0, 0, game.windowWidth, game.windowHeight), Color.White);
                game._spriteBatch.TextWithShadow(font, "Pauza", new Vector2(480, 260), Color.Red);
                game._spriteBatch.TextWithShadow(font, "Czy chcesz wyjsc z gry?\n\n\'Y\' - wyjsc z gry \n\n\'N\' - kontynuuj gre", new Vector2(440, 340), Color.Red);
            }
            game._spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
