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
        public static int aiColor = 1;
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
        public int[] EnPassant;
        public Boolean aiLeftCastling, aiRightCastling, playerLeftCastling, playerRightCastling;
        public Boolean aiCheck, playerCheck;
        public int[,] tiles;
        public int mate;

        public Board()
        {
            tiles = new int[8, 8];
            aiLeftCastling = aiRightCastling = playerLeftCastling = playerRightCastling = true;
            aiCheck = playerCheck = false;
            mate = 0;
        }

        //Used for resetting the pieces on the board
        public void ResetGame()
        {
            aiLeftCastling = aiRightCastling = playerLeftCastling = playerRightCastling = true;
            aiCheck = playerCheck = false;
            mate = 0;
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
                piece = piece * -aiColor;
            }
            else if (h == 7 || h == 6)
            {
                piece = piece * aiColor;
            }
            else
            {
                piece = 0;
            }
            return piece;
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
                    newBoard.tiles[h, w] = this.tiles[h, w];
                }
            }
            newBoard.mate = this.mate;
            newBoard.aiCheck = this.aiCheck;
            newBoard.aiLeftCastling = this.aiLeftCastling;
            newBoard.aiRightCastling = this.aiRightCastling;
            newBoard.playerCheck = this.playerCheck;
            newBoard.playerLeftCastling = this.playerLeftCastling;
            newBoard.playerRightCastling = this.playerRightCastling;
            return newBoard;
        }

        /*
         * Check if specified player is in check
         */
        public static Boolean CheckForCheck(Board board, int player, int[] king)
        {
            MoveGenerator mg = new MoveGenerator();
            List<IMove> checkMoves = mg.GetAllMovesForPlayer(board, -player);

            foreach (IMove move in checkMoves)
            {
                if (move is Move)
                {
                    Move nMove = (Move)move;
                    if (nMove.moving.Target[0] == king[0] && nMove.moving.Target[1] == king[1])
                    {
                        if (player == Board.aiColor)
                        {
                            //Console.WriteLine("ai in check");
                            board.aiCheck = true;
                            return true;
                        }
                        else
                        {
                            //Console.WriteLine("player in check");
                            board.playerCheck = true;
                            return true;
                        }

                    }
                }
            }
            return false;
            /*
            foreach (Move move in checkMovesAI)
            {
                if (move.moving.Target[0] == playerKing[0] && move.moving.Target[1] == playerKing[1])
                {
                    board.playerCheck = true;
                    Console.WriteLine("player in check");
                }
            }*/
        }

        /*
         * Check if specified player has been set in mate
         */
        public static Boolean CheckForMate(Board board, int player, int[] king)
        {
            MoveGenerator mg = new MoveGenerator();

            if ((player == Board.aiColor && board.aiCheck) || (player != Board.aiColor && board.playerCheck))
            {
                List<IMove> checkAI = mg.GetAllMovesForPlayer(board, -player);
                foreach (Move move in checkAI)
                {
                    if (move.moving.Target[0] == king[0] && move.moving.Target[1] == king[1])
                    {
                        board.mate = player;
                        //Console.WriteLine("mate for " + player);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public void CheckChecking(Board board)
        {
            int[] aiKing, playerKing;
            aiKing = playerKing = null;
            Parallel.For(0, 8, row =>
            {
                //if (king != null)
                //{
                for (int col = 0; col < 8; col++)
                {
                    if ((board.tiles[row, col] * Board.aiColor) == 6)
                    {
                        aiKing = new int[] { row, col };
                    }
                    else if ((board.tiles[row, col] * -Board.aiColor) == 6)
                    {
                        playerKing = new int[] { row, col };
                    }
                }
                //}
            });
            /*if (board.aiCheck && aiKing != null)
            {
                CheckForMate(board, Board.aiColor, aiKing);
            }
            else*/if (aiKing != null)
            {
                board.aiCheck = false;
                CheckForCheck(board, Board.aiColor, aiKing);
            }
            /*if (board.playerCheck && playerKing != null)
            {
                CheckForMate(board, -Board.aiColor, playerKing);
            }
            else*/ if (playerKing != null)
            {
                board.playerCheck = false;
                CheckForCheck(board, -Board.aiColor, playerKing);
            }
        }
        
        public void CheckMate()
        {

        }

        public static void CheckForStuff(Board board, IMove pMove)
        {
            if (pMove is Move)
            {
                Move move = (Move)pMove;
                int piece = move.moving.Piece;
                board.EnPassant = null;
                if (piece * Board.aiColor == 2)
                {
                    if (board.aiLeftCastling && move.moving.Origin[1] == 0)
                    {
                        board.aiLeftCastling = false;
                    }
                    else if (board.aiRightCastling && move.moving.Origin[1] == 7)
                    {
                        board.aiRightCastling = false;
                    }
                }
                else if (piece * Board.aiColor == -2)
                {
                    if (board.playerLeftCastling && move.moving.Origin[1] == 0)
                    {
                        board.playerLeftCastling = false;
                    }
                    else if (board.playerRightCastling && move.moving.Origin[1] == 7)
                    {
                        board.playerRightCastling = false;
                    }
                }
                else if (Math.Abs(piece) == 1)
                {
                    if (move.moving.Target[0] == 0 || move.moving.Target[0] == 7)
                    {
                        move.moving.Piece = 5 * piece;
                    }
                    else if (piece * Board.aiColor == 1 && Math.Abs(move.moving.Origin[0] - move.moving.Target[0]) == 2)
                    {
                        board.EnPassant = move.moving.Target;
                    }
                    else if (piece * Board.aiColor == -1 && Math.Abs(move.moving.Origin[0] - move.moving.Target[0]) == 2)
                    {
                        board.EnPassant = move.moving.Target;
                    }
                }
                else if (piece * Board.aiColor == 6)
                {
                    board.aiLeftCastling = false;
                    board.aiRightCastling = false;
                }
                else if (piece * Board.aiColor == -6)
                {
                    board.playerLeftCastling = false;
                    board.playerRightCastling = false;
                }
            }
        }
    }
}

