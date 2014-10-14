using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Board
    {
        Tile[,] tiles;

        public Board()
        {
            for(int h = 0; h < 8; h++){
                for (int w = 0; w < 0; w++)
                {
                    tiles[h, w] = new Tile(h, w);
                }
            }
        }

        public Tile[,] GetTiles()
        {
            return tiles;
        }

    }

    class Tile
    {
        int y, x;
        Piece owner;

        public Tile(int y, int x)
        {

        }
    }
}
