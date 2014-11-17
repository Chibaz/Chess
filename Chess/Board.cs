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
        public static int color = 1;
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
        public void ResetGame()
        {
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    tiles[h, w] = GetStartPiece(h, w);
                }
            }
        }


        //Used for getting which piece will be at the a specified tile at the start of a game
        public int GetStartPiece(int h, int w)
        {
            int piece = 0;

            //Gets which piece is supposed to be at what position
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

            //Sets the correct color of the pieces
            if (h == 0 || h == 1)
            {
                piece = piece * -color;
            }
            else if (h == 7 || h == 6)
            {
                piece = piece * color;
            }
            else
            {
                piece = 0;
            }
            return piece;
        }

        public List<IMove> GetAllMovesForPlayer(int player)
        {
            List<IMove> allMoves= new List<IMove>();
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    if (tiles[h, w]*player > 0)
                    {
                        allMoves.AddRange(GetLegalMovements(new int[] { h, w }));
                    }
                }
            }
            //Console.WriteLine("number of moves for board is :" + allMoves.Count);
            return allMoves;
        }

        public List<IMove> GetLegalMovements(int[] origin)
        {
            List<IMove> moves = new List<IMove>();
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
                if (piece == 1 || piece == 6)
                {
                    moves.AddRange(GetSpecialMoves(origin));
                }
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
            if (piece * color == -1)
            {
                moves = new String[] { "1,0" };
            }
            else if (piece * color == 1)
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

        public List<IMove> GetSpecialMoves(int[] origin)
        {
            List<IMove> specialMoves= new List<IMove>();
            int piece = GetSpecificTile(origin);
            if (piece == 1)
            {
                if (origin[0] == 6)
                {
                    Move special = new Move(origin);
                    special.Target = new int[] { origin[0] -2, origin[1] };
                    specialMoves.Add(special);
                }
                if (origin[0] == 3)
                {
                    EnPassante special = new EnPassante(origin);
                    special.Target = new int[] { origin[0] - 1, origin[1] };

                    specialMoves.Add(special);
                }
                if (origin[0] == 1)
                {
                    Move special = new Move(origin);
                    special.Target = new int[] { origin[0] + 2, origin[1] };
                    specialMoves.Add(special);
                }
                if (origin[0] == 4)
                {
                    EnPassante special = new EnPassante(origin);
                    special.Target = new int[] { origin[0] + 1, origin[1] };

                    specialMoves.Add(special);
                }
            }
            else if (piece == -1)
            {

            }
            else if (piece == 6)
            {

            }
            else if (piece == -6)
            {

            }
            return specialMoves;
        }

        public Boolean CheckForKill(Move move)
        {
            if (move.ToMove * GetSpecificTile(move.Target) < 0)
            {
                //move.ToKill = move.Target;
                return true;
            }
            return false;
        }

        public Boolean CheckForCheck(MoveGenerator move)
        {
            int[] king = {0,0};
            for (int row = 0; row < board.Tiles.GetLength(0); row++)
            {
                for(int col =0; col<board.Tiles.GetLength(0); col++) {
                    if ((Board.Game.Tiles[row, col]*color) == 6){
                        int[] king = {row, col};
                    }
                }
            }
            List<IMove> check = move.GetAllMovesForPlayer(color * -1);

            foreach (Move element in check)
            {
                if (element.moving.Target == king){
                    return true;
                }

            }
			return false;

        }

        public int GetSpecificTile(int[] tile)
        {
            return tiles[tile[0], tile[1]];
        }

        public Board CloneBoard()
        {
            Board newBoard = new Board();
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    newBoard.Tiles[h, w] = this.tiles[h, w];
                }
            }
            return newBoard;
        }
    }
}
