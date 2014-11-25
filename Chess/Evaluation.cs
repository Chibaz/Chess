using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Evaluation
    {
        private static int eBoard;
        private int wPieces, bPieces;

        public static int Evaluation(Board toEvaluate, out wPieces, out bPieces){
            eBoard = toEvaluate;
            int aiScore = 
            return 0;
        }

        public static int evaluate(Board toEvaluate, int player)
        {
            int whitescore, blackscore, wPieces, bPieces;
            whitescore = blackscore = wPieces = bPieces = 0;
            int[,] tiles = toEvaluate.tiles;
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(0); col++)
                {
                    if (tiles[row, col] * Board.aiColor == 1)
                    {
                        whitescore += pawnTable[row, col];
                        wPieces += 100;
                    }
                    else if (tiles[row, col] * Board.aiColor == -1)
                    {
                        blackscore += blackPawnTable[row, col];
                        bPieces += 100;
                    }
                    else if (tiles[row, col] * Board.aiColor == 2)
                    {
                        whitescore += rookTable[row, col];
                        wPieces += 500;
                    }
                    else if (tiles[row, col] * Board.aiColor == -2)
                    {
                        blackscore += blackRookTable[row, col];
                        bPieces += 500;
                    }
                    else if (tiles[row, col] * Board.aiColor == 3)
                    {
                        whitescore += knightTable[row, col];
                        wPieces += 300;
                    }
                    else if (tiles[row, col] * Board.aiColor == -3)
                    {
                        blackscore += blackKnightTable[row, col];
                        bPieces += 300;
                    }
                    else if (tiles[row, col] * Board.aiColor == 4)
                    {
                        whitescore += bishopTable[row, col];
                        wPieces += 300;
                    }
                    else if (tiles[row, col] * Board.aiColor == -4)
                    {
                        blackscore += blackBishopTable[row, col];
                        bPieces += 300;
                    }
                    else if (tiles[row, col] * Board.aiColor == 5)
                    {
                        whitescore += queenTable[row, col];
                        wPieces += 900;
                    }
                    else if (tiles[row, col] * Board.aiColor == -5)
                    {
                        blackscore += blackQueenTable[row, col];
                        bPieces += 900;
                    }
                    else if (tiles[row, col] * Board.aiColor == 6)
                    {
                        whitescore += kingTable[row, col] + 10000;
                    }
                    else if (tiles[row, col] * Board.aiColor == -6)
                    {
                        blackscore += blackKingTable[row, col] + 10000;
                    }
                }
            }
            whitescore += wPieces;
            blackscore += bPieces;
            if (!endGame)
            {
                
            }
            //Console.WriteLine("Iteration " + count + " White Score: " + whitescore + " Black Score: " + blackscore);

            return whitescore - blackscore;
        }
    }

            private static int[,] pawnTable =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},    
            {7, 7, 13, 23, 26, 13, 7, 7},
            {-2, -2, 4, 12, 15, 4, -2, -2},
            {-3, -3, 2, 9, 11, 2, -3, -3},
            {-4, -4, 0, 6, 8, 0, -4, -4},
            {-4, -4, 0, 4, 6, 0, -4, -4},
            {-1, -1, 1, 5, 6, 1, -1, -1},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[,] knightTable =
        {
            {-2, 2, 7, 9, 9, 7, 2, -2},
            {1, 4, 12, 13, 13, 12, 4, 1},
            {5, 11, 18, 19, 19, 18, 11, 5},
            {3, 10, 14, 14, 14, 14, 10, 3}, 
            {0, 5, 8, 9, 9, 8, 5, 0},
            {-3, 1, 3, 4, 4, 3, 1, -3},
            {-5, -3, -1, 0, 0, -1, -3, -5},
            {-7, -5, -4, -2, -2, -4, -5, -7}

        };

        private static int[,] bishopTable =
        {
            {2, 3, 4, 4, 4, 4, 3, 2},
            {4, 7, 7, 7, 7, 7, 7, 4}, 
            {3, 5, 6, 6, 6, 6, 5, 3},
            {3, 5, 7, 7, 7, 7, 5, 3}, 
            {4, 5, 6, 8, 8, 6, 5, 4}, 
            {4, 5, 5, -2, -2, 5, 5, 4},
            {5, 5, 5, 3, 3, 5, 5, 5}, 
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[,] rookTable =
        {
            {9, 9, 11, 10, 11, 9, 9, 9},
            {4, 6, 7, 9, 9, 7, 6, 4},
            {9, 10, 10, 11, 11, 10, 10, 9},
            {8, 8, 8, 9, 9, 8, 8, 8},
            {6, 6, 5, 6, 6, 5, 6, 6},
            {4, 5, 5, 5, 5, 5, 5, 4},
            {3, 4, 4, 6, 6, 4, 4, 3},
            {0, 0, 0, 2, 2, 5, 0, 0}
        };

        private static int[,] queenTable =
        {
            {2, 3, 4, 3, 4, 3, 3, 2},
            {2, 3, 4, 4, 4, 4, 3, 2},
            {3, 4, 4, 4, 4, 4, 4, 3},
            {3, 3, 4, 4, 4, 4, 3, 3}, 
            {2, 3, 3, 4, 4, 3, 3, 2}, 
            {2, 2, 2, 3, 3, 2, 2, 2},
            {2, 2, 2, 2, 2, 2, 2, 2}, 
            {0, 0, 0, 0, 0, 0, 0, 0}

        };

        private static int[,] kingTable =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}, 
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}, 
            {0, 0, 0, 0, 0, 0, 15, 0}
        };

        private static int[,] blackPawnTable =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},    
            {-1, -1, 1, 5, 6, 1, -1, -1},
            {-4, -4, 0, 4, 6, 0, -4, -4},
            {-4, -4, 0, 6, 8, 0, -4, -4},
            {-3, -3, 2, 9, 11, 2, -3, -3},
            {-2, -2, 4, 12, 15, 4, -2, -2},
            {7, 7, 13, 23, 26, 13, 7, 7},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[,] blackKnightTable =
        {
            {-7, -5, -4, -2, -2, -4, -5, -7},
            {-5, -3, -1, 0, 0, -1, -3, -5},
            {-3, 1, 3, 4, 4, 3, 1, -3},
            {0, 5, 8, 9, 9, 8, 5, 0},
            {3, 10, 14, 14, 14, 14, 10, 3},
            {5, 11, 18, 19, 19, 18, 11, 5},
            {1, 4, 12, 13, 13, 12, 4, 1},
            {-2, 2, 7, 9, 9, 7, 2, -2}
        };

        private static int[,] blackBishopTable =
        {   
            {0, 0, 0, 0, 0, 0, 0, 0},
            {5, 5, 5, 3, 3, 5, 5, 5}, 
            {4, 5, 5, -2, -2, 5, 5, 4},
            {4, 5, 6, 8, 8, 6, 5, 4},
            {3, 5, 7, 7, 7, 7, 5, 3},
            {3, 5, 6, 6, 6, 6, 5, 3},
            {4, 7, 7, 7, 7, 7, 7, 4},
            {2, 3, 4, 4, 4, 4, 3, 2}
        };

        private static int[,] blackRookTable =
        {  
            {0, 0, 0, 0, 0, 0, 0, 0},
            {3, 4, 4, 6, 6, 4, 4, 3},  
            {4, 5, 5, 5, 5, 5, 5, 4},   
            {6, 6, 5, 6, 6, 5, 6, 6},   
            {8, 8, 8, 9, 9, 8, 8, 8},   
            {9, 10, 10, 11, 11, 10, 10, 9},
            {4, 6, 7, 9, 9, 7, 6, 4},
            {9, 9, 11, 10, 11, 9, 9, 9}
        };

        private static int[,] blackQueenTable =
        {
               
            {0, 0, 0, 0, 0, 0, 0, 0},
            {2, 2, 2, 2, 2, 2, 2, 2},
            {2, 2, 2, 3, 3, 2, 2, 2},
            {2, 3, 3, 4, 4, 3, 3, 2},
            {3, 3, 4, 4, 4, 4, 3, 3},
            {3, 4, 4, 4, 4, 4, 4, 3},
            {2, 3, 4, 4, 4, 4, 3, 2},
            {2, 3, 4, 3, 4, 3, 3, 2}
        };

        private static int[,] blackKingTable =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}, 
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}, 
            {0, 0, 0, 0, 0, 0, 0, 0}
        };
            
}
