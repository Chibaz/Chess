using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    abstract class Piece
    {
        protected string name { get; set; }
        protected Boolean owner { get; set; }
        protected Boolean moveAbs { get; set; }
        protected int[,] moves { get; set; }
        protected int[,] specials { get; set; }

        public List<int[]> generateMoves(int[] position)
        {
            List<int[]> realMoves = new List<int[]>();
            foreach(int[] move in moves){
                realMoves.Add(new int[] {position[0]+move[0], position[1]+move[1]});
            }
            return realMoves;
        }
    }

    class Pawn : Piece
    {
        public Pawn(Boolean owner)
        {
            name = "pawn";
            this.owner = owner;
            moveAbs = true;
            if (owner)
            {
                moves = new int[,] { { 1, 0 } };
                specials = new int[,] { { 1, -1 }, {-1, 1} };
            }
            else
            {
                moves = new int[,] { { -1, 0 } };
                specials = new int[,] { { -1, -1 }, { -1, 1 } };
            }
        }
    }

    class Rook : Piece
    {
        public Rook(Boolean owner){
            name = "rook";
            this.owner = owner;
            moveAbs = false;
            moves = new int[,] { {1, 0}, {2, 0}, {3, 0}, {4, 0}, {5, 0}, {6, 0}, {7, 0},
                                 {-1, 0}, {-2, 0}, {-3, 0}, {-4, 0}, {-5, 0}, {-6, 0}, {-7, 0},
                                 {0, 1}, {0, 2}, {0, 3}, {0, 4}, {0, 5}, {0, 6}, {0, 7},
                                 {0, -1}, {0, -2}, {0, -3}, {0, -4}, {0, -5}, {0, -6}, {0, -7} };
        }
    }

    class Knight : Piece
    {
        public Knight(Boolean owner)
        {
            name = "knight";
            this.owner = owner;
            moveAbs = true;
            moves = new int[,] { {2, 1}, {2, -1}, {-2, 1}, {-2, -1}, {1, 2}, {1, -2}, {-1, 2}, {-1, -2} };
        }
    }

    class Bishop : Piece
    {
        public Bishop(Boolean owner)
        {
            name = "bishop";
            this.owner = owner;
            moveAbs = false;
            moves = new int[,] { {1, 1}, {2, 2}, {3, 3}, {4, 4}, {5, 5}, {6, 6}, {7, 7},
                                 {-1, -1}, {-2, -2}, {-3, -3}, {-4, -4}, {-5, -5}, {-6, -6}, {-7, -7},
                                 {7, 0}, {6, 1}, {5, 2}, {4, 3}, {3, 4}, {2, 5}, {1, 6}, {0, 7},
                                 {-7, 0}, {-6, -1}, {-5, -2}, {-4, -3}, {-3, -4}, {-2, -5}, {-1, -6}, {-0, -7} };
        }
    }
    class King : Piece
    {
        public King(Boolean owner)
        {
            name = "king";
            this.owner = owner;
            moveAbs = true;
            moves = new int[,] { {1, 0}, {-1, 0}, {0, 1}, {0, -1} };                 
        }
    }

    class Queen : Piece
    {
        public Queen(Boolean owner)
        {
            name = "queen";
            this.owner = owner;
            moveAbs = true;
            moves = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        }
    }
}
