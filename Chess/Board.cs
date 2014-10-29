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
        private int tX, tY;
        private MainWindow window;

        public Board(MainWindow window)
        {
            tiles = new Tile[8, 8];
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    Tile tile = new Tile(h, w);
                    if (h == 0 || h == 1 || h == 6 || h == 7)
                    {
                        tile.Owner = GetStartPiece(tile);
                    }
                    tiles[h, w] = tile;
                }
            }
            this.window = window;
        }

        //Used for getting which piece will be at the a specified tile at the start of a game
        public Piece GetStartPiece(Tile tile)
        {
            int h = tile.Y;
            int w = tile.X;
            Piece piece = null;
            if (h == 0)
            {
                if (w == 0 || w == 7)
                {
                    piece = new Rook(false);
                }
                else if (w == 1 || w == 6)
                {
                    piece = new Knight(false);
                }
                else if (w == 2 || w == 5)
                {
                    piece = new Bishop(false);
                }
                else if (w == 3)
                {
                    piece = new Queen(false);
                }
                else if (w == 4)
                {
                    piece = new King(false);
                }
            }
            else if (h == 1)
            {
                piece = new Pawn(false);
            }
            else if (h == 6)
            {
                piece = new Pawn(true);
            }
            else if (h == 7)
            {
                if (w == 0 || w == 7)
                {
                    piece = new Rook(true);
                }
                else if (w == 1 || w == 6)
                {
                    piece = new Knight(true);
                }
                else if (w == 2 || w == 5)
                {
                    piece = new Bishop(true);
                }
                else if (w == 3)
                {
                    piece = new Queen(true);
                }
                else if (w == 4)
                {
                    piece = new King(true);
                }
            }
            return piece;
        }

        public List<Tile> GetLegalMovements(Tile origin)
        {
            List<Tile> moves = new List<Tile>();
            Piece piece = origin.Owner;
            if (piece != null && piece.Movement) //Movement is without range limit
            {
                Console.WriteLine("relative movement");
                if(piece.Move.Contains("straight")){
                    Console.WriteLine("straight movement");
                    for (int x = origin.X + 1; x < 8; x++)
                    {
                        Console.WriteLine("checking " + origin.Y + "," + x);
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
                    for (int x = origin.X - 1; x > 0; x--)
                    {
                        Console.WriteLine("checking " + origin.Y + "," + x);
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
                    for (int y = origin.Y + 1; y < 8; y++)
                    {
                        Console.WriteLine("checking " + y + "," + origin.X);
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
                    for (int y = origin.Y - 1; y > 0; y--)
                    {
                        Console.WriteLine("checking " + y + "," + origin.X);
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
                    int xL, xR;
                    xL = xR = origin.X;
                    Boolean leftUnbroken, rightUnbroken;
                    leftUnbroken = rightUnbroken = true;
                    for(int y = origin.Y + 1; y < 8; y++){ //Lower diagonals
                        xL--;
                        xR++;
                        if(xL > 0 && tiles[y, xL].Owner == null && leftUnbroken){
                            moves.Add(tiles[y, xL]);
                        }else{
                            leftUnbroken = false;
                        }
                        if(xR < 8 && tiles[y, xR].Owner == null && rightUnbroken){
                            moves.Add(tiles[y, xR]);
                        }else{
                            rightUnbroken = false;
                        }
                    }
                    xL = xR = origin.X;
                    leftUnbroken = rightUnbroken = true;
                    for(int y = origin.Y - 1; y >= 0; y--){ //Upper diagonals
                        xL--;
                        xR++;
                        if (xL > 0 && tiles[y, xL].Owner == null && leftUnbroken)
                        {
                            moves.Add(tiles[y, xL]);
                        }else{
                            leftUnbroken = false;
                        }
                        if (xR < 8 && tiles[y, xR].Owner == null && rightUnbroken)
                        {
                            moves.Add(tiles[y, xR]);
                        }else{
                            rightUnbroken = false;
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
            Console.WriteLine("legal moves for " + origin.toString());
            foreach (Tile t in moves)
            {
                Console.WriteLine(t.toString());
            }
            return moves;
        }

        public Tile[,] GetTiles()
        {
            return tiles;
        }

        public Tile GetSpecificTile(int y, int x)
        {
            return tiles[y, x];
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

        public String toString()
        {
            String tile;
            if (owner != null)
            {
                tile = "Tile at " + y + "," + x + " contains " + owner.Name;
            }
            else
            {
                tile = "Tile at " + y + "," + x + " is empty";
            }
            return tile;
        }
    }

    public class Move
    {
        private Tile org;
        public Tile Org { get { return org;} }
        private Tile target;
        public Tile Target { get { return target; } set { target = value; } }
        private Boolean special;
        private Piece mover;
        private Piece kill;

        public Move(Tile org)
        {
            this.org = org;
            mover = org.Owner;
        }

        public void Execute()
        {
            org.Owner = null;
            target.Owner = mover;
        }
    }
}
