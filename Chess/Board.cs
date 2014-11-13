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
        public static int aiColor = 1;
        public static int[] EnPassant;
        public int[,] tiles;
        
        
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
            return newBoard;
        }
    }
}
