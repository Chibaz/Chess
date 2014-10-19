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
        }

        public void MovePieceB(int y, int x)
        {
            Tile t = tiles[y, x];
            if (t.Owner == null)
            {
                t.Owner = toMove;
                tiles[tY, tX].Owner = null;
                window.moving = false;
            }
        }

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
