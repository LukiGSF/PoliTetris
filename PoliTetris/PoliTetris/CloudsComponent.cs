using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PoliTetris
{
    public class CloudsComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private int change;
        private int direction;
        private Texture2D clouds;
        private Vector2 position;

        public CloudsComponent(Game1 game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            position = new Vector2(0, 0);
            change = 0;
            direction = 1;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            clouds = game.Content.Load<Texture2D>("spr_clouds");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            position.X--;
            if (position.X < -(clouds.Width))
                position.X = 0;

            change += direction;
            if (change >= 96)
                direction = -1;
            if (change <= 0)
                direction = 1;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game._spriteBatch.Begin();
            Color color = new Color(128 + change, 255 - change, 128 + change);
            for (int i = 0; i < 6; i++)
                game._spriteBatch.Draw(clouds, new Vector2(position.X + i * clouds.Width, 0), color * 0.8f);
            game._spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
