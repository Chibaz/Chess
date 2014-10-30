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
        private MainWindow window;

        public Board(MainWindow window)
        {
            tiles = new Tile[8, 8];
            resetGame();
            this.window = window;
        }

        //Used for resetting the pieces on the board
        public void resetGame()
        {
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    Tile tile = new Tile(h, w);
                    if (h == 0 || h == 1 || h == 6 || h == 7)
                    {
                        tile.Owner = GetStartPiece(tile, true);
                    }
                    tiles[h, w] = tile;
                }
            }
        }


        //Used for getting which piece will be at the a specified tile at the start of a game
        public Piece GetStartPiece(Tile tile, Boolean color)
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
            List<Move> movesb = new List<Move>();
            List<Tile> moves = new List<Tile>();
            Piece piece = origin.Owner;
            if (piece.Movement) //Movement is without range limit
            {
                if (piece.Move.Contains("straight"))
                {
                    moves.AddRange(GetStraightMoves(origin));
                }

                if (piece.Move.Contains("diagonal"))
                {
                    moves.AddRange(GetDiagonalMoves(origin));
                }
            }
            else //Movement is an absolute distance
            {
                moves.AddRange(GetAbsoluteMoves(origin));
            }
            return moves;
        }

        public List<Move> GetStraightMove(Move original)
        {
            Tile origin = original.Org;
            List<Move> straightMoves = new List<Move>();
            for (int y = origin.Y + 1; y < 8; y++)
            { //Horizontal lower
                Move newMove = new Move(original.Org);
                if (tiles[y, origin.X].Owner == null)
                {
                    newMove.Target = tiles[y, origin.X];
                }
                else
                {
                    CheckForKill(newMove);
                    break;
                }
                straightMoves.Add(newMove);
            }
            for (int y = origin.Y - 1; y >= 0; y--) //Horizontal upper
            {
                Move newMove = new Move(original.Org);
                if (tiles[y, origin.X].Owner == null)
                {
                    newMove.Target = tiles[y, origin.X];
                }
                else
                {
                    CheckForKill(newMove);
                    break;
                }
                straightMoves.Add(newMove);
            }
            for (int x = origin.X + 1; x < 8; x++) //Vertical right
            {
                Move newMove = new Move(original.Org);
                if (tiles[origin.Y, x].Owner == null)
                {
                    newMove.Target = tiles[origin.Y, x];
                }
                else
                {
                    CheckForKill(newMove);
                    break;
                }
                straightMoves.Add(newMove);
            }
            for (int x = origin.X - 1; x > 0; x--) //Vertical right
            {
                Move newMove = new Move(original.Org);
                if (tiles[origin.Y, x].Owner == null)
                {
                    newMove.Target = tiles[origin.Y, x];
                }
                else
                {
                    CheckForKill(newMove);
                    break;
                }
                straightMoves.Add(newMove);
            }
            return straightMoves;
        }

        public List<Move> GetDiagonalMove(Move original)
        {
            Tile origin = original.Org;
            List<Move> diagonalMoves = new List<Move>();
            int xL, xR;
            xL = xR = origin.X;
            Boolean leftUnbroken, rightUnbroken;
            leftUnbroken = rightUnbroken = true;
            Move newMove = null;
            for (int y = origin.Y + 1; y < 8; y++)
            { //Lower diagonals
                xL--;
                xR++;
                if (xL > 0 && tiles[y, xL].Owner == null && leftUnbroken)
                {
                    newMove = new Move(origin);
                    newMove.Target = tiles[y, xL];
                    diagonalMoves.Add(newMove);
                }
                else if (xL > 0 && tiles[y, xL].Owner != null && leftUnbroken)
                {
                    newMove = CheckForKill(newMove);
                    leftUnbroken = false;
                }
                if (xR < 8 && tiles[y, xR].Owner == null && rightUnbroken)
                {
                    newMove = new Move(origin);
                    newMove.Target = tiles[y, xR];
                    diagonalMoves.Add(newMove);
                }
                else if (xR < 8 && tiles[y, xR].Owner == null && rightUnbroken)
                {
                    newMove = CheckForKill(newMove);
                    rightUnbroken = false;
                }
            }
            xL = xR = origin.X;
            leftUnbroken = rightUnbroken = true;
            newMove = null;
            for (int y = origin.Y - 1; y >= 0; y--)
            { //Upper diagonals
                xL--;
                xR++;
                if (xL > 0 && tiles[y, xL].Owner == null && leftUnbroken)
                {
                    newMove = new Move(origin);
                    newMove.Target = tiles[y, xL];
                    diagonalMoves.Add(newMove);
                }
                else if (xL > 0 && tiles[y, xL].Owner != null && leftUnbroken)
                {
                    CheckForKill(newMove);
                    leftUnbroken = false;
                }
                if (xR < 8 && tiles[y, xR].Owner == null && rightUnbroken)
                {
                    newMove = new Move(origin);
                    newMove.Target = tiles[y, xR];
                    diagonalMoves.Add(newMove);
                }
                else if (xR < 8 && tiles[y, xR].Owner != null && rightUnbroken)
                {
                    CheckForKill(newMove);
                    rightUnbroken = false;
                }
            }
            return diagonalMoves;
        }


        public List<Tile> GetStraightMoves(Tile origin)
        {
            List<Tile> straightMoves = new List<Tile>();
            for (int y = origin.Y + 1; y < 8; y++)
            { //Horizontal lower
                if (tiles[y, origin.X].Owner == null)
                {
                    straightMoves.Add(tiles[y, origin.X]);
                }
                else
                {
                    //check for kill move
                    break;
                }
            }
            for (int y = origin.Y - 1; y >= 0; y--) //Horizontal upper
            {
                if (tiles[y, origin.X].Owner == null)
                {
                    straightMoves.Add(tiles[y, origin.X]);
                }
                else
                {
                    //check for kill move
                    break;
                }
            }
            for (int x = origin.X + 1; x < 8; x++) //Vertical right
            {
                if (tiles[origin.Y, x].Owner == null)
                {
                    straightMoves.Add(tiles[origin.Y, x]);
                }
                else
                {
                    //check for kill move
                    break;
                }
            }
            for (int x = origin.X - 1; x > 0; x--) //Vertical right
            {
                if (tiles[origin.Y, x].Owner == null)
                {
                    straightMoves.Add(tiles[origin.Y, x]);
                }
                else
                {
                    //check for kill move
                    break;
                }
            }
            return straightMoves;
        }

        public List<Tile> GetDiagonalMoves(Tile origin)
        {
            List<Tile> diagonalMoves = new List<Tile>();
            int xL, xR;
            xL = xR = origin.X;
            Boolean leftUnbroken, rightUnbroken;
            leftUnbroken = rightUnbroken = true;
            for (int y = origin.Y + 1; y < 8; y++)
            { //Lower diagonals
                xL--;
                xR++;
                if (xL > 0 && tiles[y, xL].Owner == null && leftUnbroken)
                {
                    diagonalMoves.Add(tiles[y, xL]);
                }
                else if(leftUnbroken)
                {
                    //Check for kill move
                    leftUnbroken = false;
                }
                if (xR < 8 && tiles[y, xR].Owner == null && rightUnbroken)
                {
                    diagonalMoves.Add(tiles[y, xR]);
                }
                else if(rightUnbroken)
                {
                    //Check for kill move
                    rightUnbroken = false;
                }
            }
            xL = xR = origin.X;
            leftUnbroken = rightUnbroken = true;
            for (int y = origin.Y - 1; y >= 0; y--)
            { //Upper diagonals
                xL--;
                xR++;
                if (xL > 0 && tiles[y, xL].Owner == null && leftUnbroken)
                {
                    diagonalMoves.Add(tiles[y, xL]);
                }
                else if(leftUnbroken)
                {
                    //Check for kill move
                    leftUnbroken = false;
                }
                if (xR < 8 && tiles[y, xR].Owner == null && rightUnbroken)
                {
                    diagonalMoves.Add(tiles[y, xR]);
                }
                else if(rightUnbroken)
                {
                    //Check for kill move
                    rightUnbroken = false;
                }
            }
            return diagonalMoves;
        }

        public List<Tile> GetAbsoluteMoves(Tile origin)
        {
            List<Tile> absMoves = new List<Tile>();
            foreach (String s in origin.Owner.Move)
            {
                String[] m = s.Split(new Char[] { ',' }, 2);
                int y = origin.Y + int.Parse(m[0]);
                int x = origin.X + int.Parse(m[1]);
                if (y >= 0 && x >= 0 && y < 8 && x < 8 && tiles[y, x].Owner == null)
                {
                    absMoves.Add(tiles[y, x]);
                }
                else if (y >= 0 && x >= 0 && y < 8 && x < 8 && tiles[y, x].Owner != null)
                {
                    //check for kill move
                }
            }
            return absMoves;
        }

        public Move CheckForKill(Move move)
        {
            if (!move.Special && move.Mover.Color != move.Target.Owner.Color)
            {
                move.Kill = move.Target;
            }
            return move;
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
        public Tile Org { get { return org; } }
        private Tile target;
        public Tile Target { get { return target; } set { target = value; } }
        private Boolean special;
        public Boolean Special { get { return special; } }
        public Piece Mover { get { return org.Owner; } }
        private Tile killPos;
        public Tile Kill { get { return killPos; } set { killPos = value; } }

        public Move(Tile org)
        {
            this.org = org;
        }

        public void Execute()
        {
            if (killPos != null)
            {
                killPos.Owner = null;
            }
            target.Owner = org.Owner;
            org.Owner = null;
            
        }
    }
}
