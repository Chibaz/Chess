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
    }

    class Tile
    {
        int x, y;
        Piece owner;
    }
}
