using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /*
     * Board is based on a 2-dimensional array of ints, pieces are defined as:
     * 1 - Pawn
     * 2 - Rook
     * 3 - Knight
     * 4 - Bishop
     * 5 - Queen
     * 6 - King
     */
    public class Board
    {

        private int[,] tiles;
        public int[,] Tiles
        {
            get
            {
                return tiles;
            }
            set
            {
                tiles = value;
            }
        }
        private static Board board;
        public static Board Game
        {
            get
            {
                if (board == null)
                {
                    board = new Board();
                }
                return board;
            }
        }

        public Board()
        {
            tiles = new int[8, 8];
        }

        //Used for resetting the pieces on the board
        public void ResetGame(int color)
        {
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    tiles[h, w] = GetStartPiece(h, w, -1);
                }
            }
        }


        //Used for getting which piece will be at the a specified tile at the start of a game
        public int GetStartPiece(int h, int w, int color)
        {
            int piece = 0;

            if (h == 1 || h == 6)
            {
                piece = 1;
            }
            else if (w == 0 || w == 7)
            {
                piece = 2;
            }
            else if (w == 1 || w == 6)
            {
                piece = 3;
            }
            else if (w == 2 || w == 5)
            {
                piece = 4;
            }
            else if (w == 3)
            {
                piece = 5;
            }
            else if (w == 4)
            {
                piece = 6;
            }

            if (h == 0 || h == 1)
            {
                piece *= color;
            }
            else if (h == 7 || h == 6)
            {
                piece *= -color;
            }
            else
            {
                piece = 0;
            }
            return piece;
        }

        public List<Move> GetLegalMovements(int[] origin)
        {
            List<Move> moves = new List<Move>();
            int piece = Math.Abs(tiles[origin[0], origin[1]]);

            if (piece == 2 || piece == 5) //Movement in straight lines
            {
                moves.AddRange(GetStraightMoves(origin));
            }
            if (piece == 4 || piece == 5) //Movement in diagonal lines
            {
                moves.AddRange(GetDiagonalMoves(origin));
            }
            if (piece == 1 || piece == 3 || piece == 6) //Movement is an absolute distance
            {
                moves.AddRange(GetAbsoluteMoves(origin));
            }
            return moves;
        }

        public List<Move> GetStraightMoves(int[] origin)
        {
            Move newMove = null;
            List<Move> straightMoves = new List<Move>();
            for (int y = origin[0] + 1; y < 8; y++) //Vertical lower
            {
                newMove = new Move(origin);
                newMove.Target = new int[] { y, origin[1] };
                if (GetSpecificTile(newMove.Target) == 0)
                {
                    straightMoves.Add(newMove);
                }
                else
                {
                    if (CheckForKill(newMove))
                    {
                        straightMoves.Add(newMove);
                    }
                    break;
                }
            }
            for (int y = origin[0] - 1; y >= 0; y--) //Vertical upper
            {
                newMove = new Move(origin);
                newMove.Target = new int[] { y, origin[1] };
                if (GetSpecificTile(newMove.Target) == 0)
                {
                    straightMoves.Add(newMove);
                }
                else
                {
                    if (CheckForKill(newMove))
                    {
                        straightMoves.Add(newMove);
                    }
                    break;
                }
            }
            for (int x = origin[1] + 1; x < 8; x++) //Horizontal right
            {
                newMove = new Move(origin);
                newMove.Target = new int[] { origin[0], x };
                if (GetSpecificTile(newMove.Target) == 0)
                {
                    straightMoves.Add(newMove);
                }
                else
                {
                    if (CheckForKill(newMove))
                    {
                        straightMoves.Add(newMove);
                    }
                    break;
                }
            }
            for (int x = origin[1] - 1; x >= 0; x--) //Horizontal left
            {
                newMove = new Move(origin);
                newMove.Target = new int[] { origin[0], x };
                if (GetSpecificTile(newMove.Target) == 0)
                {
                    straightMoves.Add(newMove);
                }
                else
                {
                    if (CheckForKill(newMove))
                    {
                        straightMoves.Add(newMove);
                    }
                    break;
                }
            }
            return straightMoves;
        }

        public List<Move> GetDiagonalMoves(int[] origin)
        {
            List<Move> diagonalMoves = new List<Move>();
            int xL, xR;
            xL = xR = origin[1];
            Boolean leftUnbroken, rightUnbroken;
            leftUnbroken = rightUnbroken = true;
            Move newMove = null;
            for (int y = origin[0] + 1; y < 8; y++) //Lower diagonals
            {
                xL--;
                xR++;
                newMove = new Move(origin);
                newMove.Target = new int[] { y, xL };
                if (xL > 0 && tiles[y, xL] == 0 && leftUnbroken)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xL > 0 && leftUnbroken)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    leftUnbroken = false;
                }
                newMove = new Move(origin);
                newMove.Target = new int[] { y, xR };
                if (xR < 8 && tiles[y, xR] == 0 && rightUnbroken)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xR < 8 && rightUnbroken)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    rightUnbroken = false;
                }
            }
            xL = xR = origin[1];
            leftUnbroken = rightUnbroken = true;
            for (int y = origin[0] - 1; y >= 0; y--) //Upper diagonals
            {
                xL--;
                xR++;
                newMove = new Move(origin);
                newMove.Target = new int[] { y, xL };
                if (xL > 0 && tiles[y, xL] == 0 && leftUnbroken)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xL > 0 && leftUnbroken)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    leftUnbroken = false;
                }
                newMove = new Move(origin);
                newMove.Target = new int[] { y, xR };
                if (xR < 8 && tiles[y, xR] == 0 && rightUnbroken)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xR < 8 && rightUnbroken)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    rightUnbroken = false;
                }
            }
            return diagonalMoves;
        }

        public List<Move> GetAbsoluteMoves(int[] origin)
        {
            String[] moves = null;
            int piece = GetSpecificTile(origin);
            List<Move> absMoves = new List<Move>();
            if (piece == -1)
            {
                moves = new String[] { "1,0" };
            }
            else if (piece == 1)
            {
                moves = new String[] { "-1,0" };
            }
            else if (Math.Abs(piece) == 3)
            {
                moves = new String[] { "2,1", "1,2", "2,-1", "1,-2", "-2,1", "-1,2", "-2,-1", "-1,-2" };
            }
            else if (Math.Abs(piece) == 6)
            {
                moves = new String[] { "1,0", "-1,0", "0,1", "0,-1", "1,1", "1,-1", "-1,1", "-1,-1" };
            }
            foreach (String s in moves)
            {
                Move newMove = new Move(origin);
                String[] m = s.Split(new Char[] { ',' }, 2);
                int y = origin[0] + int.Parse(m[0]);
                int x = origin[1] + int.Parse(m[1]);
                if (y >= 0 && x >= 0 && y < 8 && x < 8)
                {
                    newMove.Target = new int[] { y, x };
                    if (GetSpecificTile(newMove.Target) == 0)
                    {
                        absMoves.Add(newMove);
                    }
                    else if (CheckForKill(newMove))
                    {
                        absMoves.Add(newMove);
                    }
                }
            }
            return absMoves;
        }

        public Boolean CheckForKill(Move move)
        {
            if (!move.Special && move.ToMove * GetSpecificTile(move.Target) < 0)
            {
                move.ToKill = move.Target;
                return true;
            }
            return false;
        }

        public int GetSpecificTile(int[] tile)
        {
            return tiles[tile[0], tile[1]];
        }
    }

    public class Move
    {
        private int[] origin;
        public int[] Origin { get { return origin; } }
        private int[] target;
        public int[] Target { get { return target; } set { target = value; } }
        private Boolean special;
        public Boolean Special { get { return special; } }
        private int toMove;
        public int ToMove { get { return toMove; } set { toMove = value; } }
        private int[] toKill;
        public int[] ToKill { get { return toKill; } set { toKill = value; } }

        public Move(int[] origin)
        {
            this.origin = origin;
            toMove = Board.Game.Tiles[origin[0], origin[1]];
        }

        public void Execute()
        {
            int[,] tiles = Board.Game.Tiles;
            if (toKill != null)
            {
                tiles[toKill[0], toKill[1]] = 0;
            }
            tiles[target[0], target[1]] = toMove;
            tiles[origin[0], origin[1]] = 0;
        }
    }
}
