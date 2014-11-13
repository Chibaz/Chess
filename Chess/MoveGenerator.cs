using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class MoveGenerator
    {
        public List<IMove> GetAllMovesForPlayer(int player)
        {
            List<IMove> allMoves = new List<IMove>();
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    if (Board.Game.tiles[h, w] * player > 0)
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
            int piece = Math.Abs(Board.Game.tiles[origin[0], origin[1]]);

            if (piece == 1)
            {
                moves.AddRange(GetPawnMoves(origin));
            }
            if (piece == 2 || piece == 5) //Movement in straight lines
            {
                moves.AddRange(GetStraightMoves(origin));
            }
            if (piece == 4 || piece == 5) //Movement in diagonal lines
            {
                moves.AddRange(GetDiagonalMoves(origin));
            }
            if (piece == 3 || piece == 6) //Movement is an absolute distance
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, origin[1] };
                if (Board.Game.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, origin[1] };
                if (Board.Game.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { x, origin[1] };
                if (Board.Game.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { x, origin[1] };
                if (Board.Game.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xL > 0 && Board.Game.tiles[y, xL] == 0 && leftUnbroken)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xR < 8 && Board.Game.tiles[y, xR] == 0 && rightUnbroken)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xL > 0 && Board.Game.tiles[y, xL] == 0 && leftUnbroken)
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
                newMove = new Move(origin, Board.Game.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xR < 8 && Board.Game.tiles[y, xR] == 0 && rightUnbroken)
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
            int piece = Board.Game.tiles[origin[0], origin[1]];
            List<Move> absMoves = new List<Move>();
            if (Math.Abs(piece) == 3)
            {
                moves = new String[] { "2,1", "1,2", "2,-1", "1,-2", "-2,1", "-1,2", "-2,-1", "-1,-2" };
            }
            else if (Math.Abs(piece) == 6)
            {
                moves = new String[] { "1,0", "-1,0", "0,1", "0,-1", "1,1", "1,-1", "-1,1", "-1,-1" };
            }
            foreach (String s in moves)
            {
                Move newMove = new Move(origin, piece);
                String[] m = s.Split(new Char[] { ',' }, 2);
                int y = origin[0] + int.Parse(m[0]);
                int x = origin[1] + int.Parse(m[1]);
                if (y >= 0 && x >= 0 && y < 8 && x < 8)
                {
                    newMove.moving.Target = new int[] { y, x };
                    if (Board.Game.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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

        public List<IMove> GetPawnMoves(int[] origin)
        {
            int piece = Board.Game.tiles[origin[0], origin[1]];
            List<IMove> pawnMoves = new List<IMove>();
            int direction = piece * Board.aiColor;
            Move newMove;

            if (Board.Game.tiles[origin[0] - direction, origin[1]] == 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - 1, origin[1] };
                pawnMoves.Add(newMove);
            }
            if (Board.Game.tiles[origin[0] - (2 * direction), origin[1]] == 0 && origin[0] == 6)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - (2 * direction), origin[1] };
                pawnMoves.Add(newMove);
            }
            if (Board.Game.tiles[origin[0] - direction, origin[1] + 1] * piece < 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] + 1 };
                newMove.killing.Position = new int[] { origin[0] - direction, origin[1] + 1 };
                newMove.killing.Piece = Board.Game.tiles[origin[0] - direction, origin[1] + 1];
                pawnMoves.Add(newMove);
            }
            if (Board.Game.tiles[origin[0] - direction, origin[1] - 1] * piece < 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] - 1 };
                newMove.killing.Position = new int[] { origin[0] - direction, origin[1] - 1 };
                newMove.killing.Piece = Board.Game.tiles[origin[0] - direction, origin[1] - 1];
                pawnMoves.Add(newMove);
            }
            if (Board.EnPassant != null)
            {
                if (Board.Game.tiles[Board.EnPassant[0], Board.EnPassant[1]] * piece < 0)
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[]{Board.EnPassant[0] - direction, Board.EnPassant[1]};
                    newMove.killing.Position = Board.EnPassant;
                    newMove.killing.Piece = Board.Game.tiles[Board.EnPassant[0], Board.EnPassant[1]];
                    pawnMoves.Add(newMove);
                }
            }
            return pawnMoves;
            /*if(piece * Board.aiColor == -1)
            {
                if (Board.Game.tiles[origin[0] + 1, origin[1]] == 0)
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { origin[0] - 1, origin[1] };
                    pawnMoves.Add(newMove);
                }
                if (Board.Game.tiles[origin[0] - 2, origin[1]] == 0)
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { origin[0] - 2, origin[1] };
                    pawnMoves.Add(newMove);
                }
                if (Board.Game.tiles[origin[0] - 1, origin[1] + 1] * piece < 0)
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { origin[0] + 1, origin[1] + 1 };
                    newMove.killing.Position = new int[] { origin[0] + 1, origin[1] + 1 };
                    newMove.killing.Piece = Board.Game.tiles[origin[0] + 1, origin[1] + 1];
                    pawnMoves.Add(newMove);
                }
                if (Board.Game.tiles[origin[0] - 1, origin[1] - 1] * piece < 0)
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { origin[0] - 1, origin[1] - 1 };
                    newMove.killing.Position = new int[] { origin[0] - 1, origin[1] - 1 };
                    newMove.killing.Piece = Board.Game.tiles[origin[0] - 1, origin[1] - 1];
                    pawnMoves.Add(newMove);
                }
            }*/
        }

        public Boolean CheckForKill(Move move)
        {
            if ((move.moving.Piece * Board.Game.tiles[move.moving.Target[0], move.moving.Target[1]]) < 0)
            {
                move.killing.Position = move.moving.Target;
                move.killing.Piece = move.moving.Piece;
                return true;
            }
            return false;
        }
    }
}
