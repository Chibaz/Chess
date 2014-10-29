using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Board
    {
        private Tile[,] tiles;
        private List<Tile> legalMoves;
        private Piece toMove;
        private int tX, tY;
        private MainWindow window;

        public Board(MainWindow window)
        {
            tiles = new Tile[8, 8];
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    Tile tile = new Tile(h + 1, w + 1);
                    if (h == 0 || h == 1 || h == 6 || h == 7)
                    {
                        tile.Owner = GetStartPiece(tile);
                    }
                    tiles[h, w] = tile;
                }
            }
            this.window = window;
        }

        //Sets the coordinates from which a piece will move
        public void MovePieceA(int y, int x)
        {
            Tile t = tiles[y, x];
            if (t.Owner != null)
            {
                tY = y;
                tX = x;
                toMove = t.Owner;
                window.moving = true;
            }
            legalMoves = GetLegalMovements(y, x);
        }

        //Moves a specified piece to a different location
        public int[] MovePieceB(int y, int x)
        {
            Tile t = tiles[y, x];
            if (t.Owner == null)
            {
                t.Owner = toMove;
                tiles[tY, tX].Owner = null;
                window.moving = false;
            }
            return new int[] { tY, tX };
        }

        //Used for getting which piece will be at the a specified tile at the start of a game
        public Piece GetStartPiece(Tile tile)
        {
            int h = tile.Y;
            int w = tile.X;
            Piece piece = null;
            if (h == 1)
            {
                if (w == 1 || w == 8)
                {
                    piece = new Rook(false);
                }
                else if (w == 2 || w == 7)
                {
                    piece = new Knight(false);
                }
                else if (w == 3 || w == 6)
                {
                    piece = new Bishop(false);
                }
                else if (w == 4)
                {
                    piece = new Queen(false);
                }
                else if (w == 5)
                {
                    piece = new King(false);
                }
            }
            else if (h == 2)
            {
                piece = new Pawn(false);
            }
            else if (h == 7)
            {
                piece = new Pawn(true);
            }
            else if (h == 8)
            {
                if (w == 1 || w == 8)
                {
                    piece = new Rook(true);
                }
                else if (w == 2 || w == 7)
                {
                    piece = new Knight(true);
                }
                else if (w == 3 || w == 6)
                {
                    piece = new Bishop(true);
                }
                else if (w == 4)
                {
                    piece = new Queen(true);
                }
                else if (w == 5)
                {
                    piece = new King(true);
                }
            }
            return piece;
        }

        public List<Tile> GetLegalMovements(int originY, int originX)
        {
            Tile origin = tiles[originY, originX];
            List<Tile> moves = new List<Tile>();
            Piece piece = origin.Owner;
            if (piece != null && piece.Movement) //Movement is without range limit
            {
                Console.WriteLine("relative movement");
                if(piece.Move.Contains("straight")){
                    Console.WriteLine("straight movement");
                    for (int x = origin.X+1; x < (8 - origin.X); x++)
                    {
                        Tile target = tiles[origin.Y, x];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int x = origin.X - 1; x >= 0; x--)
                    {
                        Tile target = tiles[origin.Y, x];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int y = origin.Y + 1; y < (8 - origin.Y); y++)
                    {
                        Tile target = tiles[y, origin.X];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int y = origin.Y - 1; y >= 0; y--)
                    {
                        Tile target = tiles[y, origin.X];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if(piece.Move.Contains("diagonal")){
                    Console.WriteLine("diagonal movement");
                    for (int y = origin.Y + 1; y < (8 - origin.Y); y++)
                    {
                        Tile target = tiles[y, y];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int y = origin.Y - 1; y >= (8 - origin.Y); y--)
                    {
                        Tile target = tiles[y, y];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int x = origin.X + 1; x < (8 - origin.X); x++)
                    {
                        Tile target = tiles[x, x];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int x = origin.X + 1; x < (8 - origin.X); x--)
                    {
                        Tile target = tiles[x, x];
                        if (target.Owner == null)
                        {
                            moves.Add(target);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else //Movement is an absolute distance
            {
                Console.WriteLine("absolute movement");
                foreach (Tile t in tiles)
                {
                    moves.Add(t);
                }
            }
            return moves;
        }

        public Tile[,] GetTiles()
        {
            return tiles;
        }
    }  

    public class Tile
    {
        protected int y;
        public int Y { get { return y; } }
        protected int x;
        public int X { get { return x; } }
        protected Piece owner;
        public Piece Owner { get { return owner; } set { owner = value; } }

        public Tile(int y, int x)
        {
            this.y = y;
            this.x = x;
        }
    }
}
