using System;
using System.Collections.Generic;
using System.Text;

namespace PoliTetris
{
    public class Player
    {
        public long score;
        public int rows;
        public int level;
        public string nickname;

        public Player()
        {
            score = 0;
            rows = 0;
            level = 1;
        }
    }
}
