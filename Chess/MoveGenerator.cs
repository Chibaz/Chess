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
            if (moveBoard.mate != 0)
            {
                Console.WriteLine("hit leaf");
                return allMoves;
            }/*else if(Board.CheckForCheck(moveBoard, player){
                return allMoves;
            }*/
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

        public List<IMove> GetStraightMoves(int[] origin)
        {
            Move newMove = null;
            List<IMove> straightMoves = new List<IMove>();
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

        public List<IMove> GetDiagonalMoves(int[] origin)
        {
            int xL, xR;
            xL = xR = origin[1];
            Move newMove = null;
            List<IMove> diagonalMoves = new List<IMove>();
            for (int y = origin[0] + 1; y < 8; y++) //Lower-Left diagonals
            {
                xL--;
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xL >= 0 && Board.Game.tiles[y, xL] == 0)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xL >= 0)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    break;
                }
            }
            for (int y = origin[0] + 1; y < 8; y++) //Lower-Right diagonals
            {
                xR++;
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xR };
                if (xR < 8 && moveBoard.tiles[y, xR] == 0)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xR <= 7)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    break;
                }
            }

            xL = xR = origin[1];
            //leftUnbroken = rightUnbroken = true;
            for (int y = origin[0] - 1; y >= 0; y--) //Upper-Left diagonals
            {
                xL--;
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xL };
                if (xL >= 0 && moveBoard.tiles[y, xL] == 0)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xL >= 0)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    break;
                }
            }
            for (int y = origin[0] - 1; y >= 0; y--){ //Upper-Right diagonals
                xR++;
                newMove = new Move(origin, moveBoard.tiles[origin[0], origin[1]]);
                newMove.moving.Target = new int[] { y, xR };
                if (xR < 8 && moveBoard.tiles[y, xR] == 0)
                {
                    diagonalMoves.Add(newMove);
                }
                else if (xR <= 7)
                {
                    if (CheckForKill(newMove))
                    {
                        diagonalMoves.Add(newMove);
                    }
                    break;
                }
            }
            return diagonalMoves;
        }

        public List<IMove> GetAbsoluteMoves(int[] origin)
        {
            String[] moves = null;
            int piece = moveBoard.tiles[origin[0], origin[1]];
            List<IMove> absMoves = new List<IMove>();
            if (Math.Abs(piece) == 3)
            {
                moves = new String[] { "2,1", "1,2", "2,-1", "1,-2", "-2,1", "-1,2", "-2,-1", "-1,-2" };
            }
            else if (Math.Abs(piece) == 6)
            {
                moves = new String[] { "1,0", "-1,0", "0,1", "0,-1", "1,1", "1,-1", "-1,1", "-1,-1" };
                absMoves.AddRange(GenerateCastling(piece / Math.Abs(piece)));
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
                pawnMoves.Add(newMove);
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
            }
            //Kill piece right
            if (origin[1] < 7 && moveBoard.tiles[origin[0] - direction, origin[1] + 1] * piece < 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] + 1 };
                newMove.killing = new TakenPiece(newMove.moving.Target, piece * -1);
                //newMove.killing.Position = new int[] { origin[0] - direction, origin[1] + 1 };
                //newMove.killing.Piece = moveBoard.tiles[origin[0] - direction, origin[1] + 1];
                //CheckForPromotion(newMove);
                pawnMoves.Add(newMove);
            }
            //Kill piece left
            if (origin[1] > 0 && moveBoard.tiles[origin[0] - direction, origin[1] - 1] * piece < 0)
            {
                newMove = new Move(origin, piece);
                newMove.moving.Target = new int[] { origin[0] - direction, origin[1] - 1 };
                newMove.killing = new TakenPiece(newMove.moving.Target, piece * -1);
                //newMove.killing.Position = new int[] { origin[0] - direction, origin[1] - 1 };
                //newMove.killing.Piece = Board.Game.tiles[origin[0] - direction, origin[1] - 1];
                //CheckForPromotion(newMove);
                pawnMoves.Add(newMove);
            }
            //EnPassant
            if (moveBoard.EnPassant != null)
            {
                if (moveBoard.tiles[moveBoard.EnPassant[0], moveBoard.EnPassant[1]] * Board.aiColor == -1 && origin[0] == 3 && (moveBoard.EnPassant[1] + 1 == origin[1] || moveBoard.EnPassant[1] - 1 == origin[1]))
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { moveBoard.EnPassant[0] - direction, moveBoard.EnPassant[1] };
                    newMove.killing = new TakenPiece(moveBoard.EnPassant, piece * -1);
                    //newMove.killing.Position = moveBoard.EnPassant;
                    //newMove.killing.Piece = moveBoard.tiles[moveBoard.EnPassant[0], moveBoard.EnPassant[1]];
                    pawnMoves.Add(newMove);
                }
                else if (moveBoard.tiles[moveBoard.EnPassant[0], moveBoard.EnPassant[1]] * Board.aiColor == 1 && origin[0] == 4 && (moveBoard.EnPassant[1] + 1 == origin[1] || moveBoard.EnPassant[1] - 1 == origin[1]))
                {
                    newMove = new Move(origin, piece);
                    newMove.moving.Target = new int[] { moveBoard.EnPassant[0] - direction, moveBoard.EnPassant[1] };
                    newMove.killing = new TakenPiece(moveBoard.EnPassant, piece * -1);
                    //newMove.killing.Position = moveBoard.EnPassant;
                    //newMove.killing.Piece = moveBoard.tiles[moveBoard.EnPassant[0], moveBoard.EnPassant[1]];
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

        
        public List<IMove> GenerateCastling(int player)
        {
            List<IMove> castling = new List<IMove>();
            Castling castle = null;
            int y;
            Boolean leftCastle, rightCastle;
            if (player == Board.aiColor)
            {
                y = 7;
                leftCastle = moveBoard.aiLeftCastling;
                rightCastle = moveBoard.aiRightCastling;
            }else{
                y = 0;
                leftCastle = moveBoard.playerLeftCastling;
                rightCastle = moveBoard.playerRightCastling;
            }
                int king = 6 * player;
                if (leftCastle && moveBoard.tiles[y, 0] == 2*player && moveBoard.tiles[y, 1] == 0 && moveBoard.tiles[y, 2] == 0 && moveBoard.tiles[y, 3] == 0 && moveBoard.tiles[y, 4] == 6)
                {
                    castle = new Castling(king, 0);
                    castling.Add(castle);
                }
                if (rightCastle && moveBoard.tiles[y, 7] == 2*player && moveBoard.tiles[y, 6] == 0 && moveBoard.tiles[y, 5] == 0 && moveBoard.tiles[y, 4] == 6)
                {
                    castle = new Castling(king, 7);
                    castling.Add(castle);
                }
            /*}
            else
            {
                int king = 6 * -Board.aiColor;
                if (moveBoard.aiLeftCastling && moveBoard.tiles[0, 1] == 0 && moveBoard.tiles[0, 2] == 0 && moveBoard.tiles[0, 3] == 0)
                {
                    castle = new Castling(king, 0);
                    castling.Add(castle);
                }
                if (moveBoard.aiRightCastling && moveBoard.tiles[7, 6] == 0 && moveBoard.tiles[7, 5] == 0)
                {
                    castle = new Castling(king, 7);
                    castling.Add(castle);
                }
            }*/
            return castling;
        }

        /*
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
        }*/

        public Boolean CheckForKill(Move move)
        {
            int piece = moveBoard.tiles[move.moving.Target[0], move.moving.Target[1]];
            if ((move.moving.Piece * moveBoard.tiles[move.moving.Target[0], move.moving.Target[1]]) < 0)
            {
                move.killing = new TakenPiece(move.moving.Target, piece);
                //Console.WriteLine("can take " + move.killing.Piece + " at " + move.killing.Position[0] + "," + move.killing.Position[1]);
                //move.killing.Position = move.moving.Target;
                //move.killing.Piece = move.moving.Piece;
                return true;
            }
            return false;
        }
    }
}
