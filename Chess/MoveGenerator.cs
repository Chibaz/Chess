using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class MoveGenerator
    {
        private Board moveBoard;

        public List<IMove> GetAllMovesForPlayer(Board board, int player)
        {
            moveBoard = board;
            List<IMove> allMoves = new List<IMove>();
            Parallel.For (0; 8; h =>
            {
                for (int w = 0; w < 8; w++)
                {
                    if (Board.Game.tiles[h, w] * player > 0)
                    {
                        allMoves.AddRange(GetLegalMovements(new int[] { h, w }));
                    }
                }
            }
            );
            //Console.WriteLine("number of moves for board is :" + allMoves.Count);
            return allMoves;
        }

        public List<IMove> GetLegalMovements(int[] origin)
        {
            List<IMove> moves = new List<IMove>();
            int piece = Math.Abs(moveBoard.tiles[origin[0], origin[1]]);

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
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, origin[1] };
                if (moveBoard.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                int[] target = new int[] { y, origin[1] };
                newMove.moving.Target = target;
                if (moveBoard.tiles[target[0], target[1]] == 0)
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
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                int[] target = new int[] { origin[0], x };
                newMove.moving.Target = target;
                if (moveBoard.tiles[target[0], target[1]] == 0)
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
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                int[] target = new int[] { origin[0], x };
                newMove.moving.Target = target;
                if (moveBoard.tiles[target[0], target[1]] == 0)
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
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xL >= 0 && Board.Game.tiles[y, xL] == 0 && leftUnbroken)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xL >= 0 && leftUnbroken)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    leftUnbroken = false;
                }
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xR };
                if (xR < 8 && moveBoard.tiles[y, xR] == 0 && rightUnbroken)
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
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xL >= 0 && moveBoard.tiles[y, xL] == 0 && leftUnbroken)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xL >= 0 && leftUnbroken)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    leftUnbroken = false;
                }
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xR };
                if (xR < 8 && moveBoard.tiles[y, xR] == 0 && rightUnbroken)
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
            int piece = moveBoard.tiles[origin[0], origin[1]];
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
                    if (moveBoard.tiles[newMove.moving.Target[0], newMove.moving.Target[1]] == 0)
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
            int piece = moveBoard.tiles[origin[0], origin[1]];
            List<IMove> pawnMoves = new List<IMove>();
            int direction = piece * Board.aiColor;
            Move newMove;

            //Move one space
            if (moveBoard.tiles[origin[0] - direction, origin[1]] == 0) 
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] };
                //CheckForPromotion(newMove);
                pawnMoves.Add(newMove);
            }
            //Move ahead two spaces from start
            if ((origin[0] == 6 && piece * Board.aiColor == 1) && moveBoard.tiles[origin[0] - (2 * direction), origin[1]] == 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - (2 * direction), origin[1] };
                pawnMoves.Add(newMove);
            }
            if ((origin[0] == 1 && piece * Board.aiColor == -1) && moveBoard.tiles[origin[0] - (2 * direction), origin[1]] == 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - (2 * direction), origin[1] };
                pawnMoves.Add(newMove);
            }
            //Kill piece right
            if (origin[1] < 7 && moveBoard.tiles[origin[0] - direction, origin[1] + 1] * piece == -1) 
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] + 1 };
                newMove.killing.Position = new int[] { origin[0] - direction, origin[1] + 1 };
                newMove.killing.Piece = moveBoard.tiles[origin[0] - direction, origin[1] + 1];
                //CheckForPromotion(newMove);
                pawnMoves.Add(newMove);
            }
            //Kill piece left
            else if (origin[1] > 0 && moveBoard.tiles[origin[0] - direction, origin[1] - 1] * piece == -1) 
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] - 1 };
                newMove.killing.Position = new int[] { origin[0] - direction, origin[1] - 1 };
                newMove.killing.Piece = Board.Game.tiles[origin[0] - direction, origin[1] - 1];
                //CheckForPromotion(newMove);
                pawnMoves.Add(newMove);
            }
            //EnPassant
            if (moveBoard.EnPassant != null)
            {
                if (moveBoard.tiles[moveBoard.EnPassant[0], moveBoard.EnPassant[1]] * piece == -1)
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { moveBoard.EnPassant[0] - direction, moveBoard.EnPassant[1] };
                    newMove.killing.Position = moveBoard.EnPassant;
                    newMove.killing.Piece = moveBoard.tiles[moveBoard.EnPassant[0], moveBoard.EnPassant[1]];
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

        public void CheckForPromotion(Move move)
        {
            if (move.moving.Piece * Board.aiColor == 1 && move.moving.Target[0] == 0)
            {
                move.moving.Piece *= 5;
            }
            else if (move.moving.Piece * Board.aiColor == -1 && move.moving.Target[0] == 7)
            {
                move.moving.Piece *= 5;
            }
        }

        public Boolean CheckForKill(Move move)
        {
            if ((move.moving.Piece * moveBoard.tiles[move.moving.Target[0], move.moving.Target[1]]) < 0)
            {
                move.killing.Position = move.moving.Target;
                move.killing.Piece = move.moving.Piece;
                return true;
            }
            return false;
        }
    }
}
