using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PoliTetris
{
    public class GameBoard
    {
        private int[,] tiles;
        private int width;
        private int height;
        private Vector2 position;
        public void Clear()
        {
            tiles = new int[width, height];
        }

        public GameBoard(int width, int height, Vector2 position)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            Clear();
        }

        public Vector2 InsertBlock(BlockElement block)
        {
            return new Vector2(((int)width / 2) - 1, 0);
        }

        public void Merge(BlockElement block)
        {
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                {
                    int x = Convert.ToInt32(i + block.position.X);
                    int y = Convert.ToInt32((j + block.position.Y));
                    if ((block.Tiles[i, j] > 0) && (y >= 0))
                        tiles[x, y] = block.Tiles[i, j];
                }
        }
        public bool Collision(BlockElement block, Vector2 position)
        {
            bool collision = false;
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    if ((block.Tiles[i, j] > 0) &&
                       ((j + position.Y >= height) || (i + position.X < 0) || (i + position.X >= width) ||
                       (tiles[Convert.ToInt32(i + position.X), Convert.ToInt32(j + position.Y)] > 0)))
                        collision = true;
            return collision;
        }

        public int RemoveRows()
        {
            int count = 0;
            for (int row = 0; row < height; row++)
            {
                bool complete = true;
                for (int i = 0; i <= (width - 1); i++)
                    if (tiles[i, row] == 0)
                        complete = false;
                if (complete)
                {
                    for (int j = (row - 1); j > 0; j--)
                        for (int i = 0; i < width; i++)
                            tiles[i, j + 1] = tiles[i, j];
                    count++;
                }
            }
            return count;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D[] sprites)
        {
            for (int j = 0; j <= (height - 1); j++)
                for (int i = 0; i <= (width - 1); i++)
                    if (tiles[i, j] != 0)
                        spriteBatch.Draw(sprites[tiles[i, j] - 1], new Vector2(position.X + i * sprites[0].Width, position.Y + j * sprites[0].Height), Color.White);
        }

    }
}
