using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PoliTetris
{
    public class Scene
    {
        private List<GameComponent> components;
        private Game1 game;

        public void AddComponent(GameComponent component)
        {
            components.Add(component);
            if (!game.Components.Contains(component))
                game.Components.Add(component);
        }

        public Scene(Game1 robotrisGame, params GameComponent[] components)
        {
            this.game = robotrisGame;
            this.components = new List<GameComponent>();
            foreach (GameComponent component in components)
            {
                AddComponent(component);
            }
        }

        public GameComponent[] ReturnComponents()
        {
            return components.ToArray();
        }

    }
}
