using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PoliTetris
{
    public class BlockElement
    {
        public int[,] Tiles { get; private set; }
        public Vector2 position;
        private Random randomGenerator;


        private int[,] copyTiles(int[,] tiles)
        {
            int[,] newTiles = new int[4, 4];
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    newTiles[i, j] = tiles[i, j];
            return newTiles;
        }

        private void GenerateSprites()
        {
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    if (Tiles[i, j] > 0)
                        Tiles[i, j] = randomGenerator.Next(1, 16);
        }

        public BlockElement(int[,] tiles)
        {
            Tiles = copyTiles(tiles);
            position = new Vector2(0, 0);
            randomGenerator = new Random();
            GenerateSprites();
        }

        public void Fall()
        {
            position.Y++;
        }

        public void Rotate()
        {
            int[,] a = copyTiles(Tiles);
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                    Tiles[x, y] = a[y, 3 - x];
        }
        public void Draw(Vector2 border, BetterSpriteBatch spriteBatch, Texture2D[] sprites)
        {
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    if (Tiles[i, j] > 0)
                        spriteBatch.Draw(sprites[Tiles[i, j] - 1], new Vector2(border.X + (i + position.X) * sprites[0].Width,
                        border.Y + (j + position.Y) * sprites[0].Height), Color.White);
        }
    }
}
