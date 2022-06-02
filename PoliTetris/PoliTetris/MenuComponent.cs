using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoliTetris
{
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;

        public MenuComponent(Game1 game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
