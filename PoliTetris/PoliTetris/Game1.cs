using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PoliTetris
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public BetterSpriteBatch _spriteBatch;
        public KeyboardState keyboardState, previousKeyboardState;
        public Scene menu, level;
        private Texture2D blocksBackground;
        public SpriteFont fontBlox;
        private SoundEffect sound;
        public Song music;
        public int windowWidth = 1280, windowHeight = 720;
        public Player player;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            CloudsComponent clouds = new CloudsComponent(this);
            LevelComponent level = new LevelComponent(this);
            Components.Add(clouds);
            Components.Add(level);
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            MenuComponent menu = new MenuComponent(this);
            base.Initialize();
        }


        public void ChangeMusic(Song song)
        {
            if (MediaPlayer.Queue.ActiveSong != song)
                MediaPlayer.Play(song);
        }


        protected override void LoadContent()
        {
            _spriteBatch = new BetterSpriteBatch(GraphicsDevice);
            blocksBackground = Content.Load<Texture2D>("background_blocks");
            fontBlox = Content.Load<SpriteFont>("font_blox");
            sound = Content.Load<SoundEffect>("alarm");
            music = Content.Load<Song>("robot");
            MediaPlayer.Play(music);
            ChangeMusic(music);
            sound.Play();
            // TODO: use this.Content to load your game content here
        }


        public bool NewKey(Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            previousKeyboardState = keyboardState;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(blocksBackground, new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
