using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Evaluation
    {
        private static int[,] eBoard;
        private static int aiPieces, playerPieces;

        public static int evaluate(Board toEvaluate/*, int depth, int lastAI, int lastPlayer, out int wPieces, out int bPieces*/)
        {
            eBoard = toEvaluate.tiles;
            aiPieces = playerPieces = 0;
            int aiScore = evaluateSide(Board.aiColor);
            int playerScore = evaluateSide(-Board.aiColor);
            /*
            if (aiPieces < lastAI)
            {
                aiScore -= 100 * depth; 
            }
            if (playerPieces < lastPlayer)
            {
                aiScore += 100 * depth;
            }
            wPieces = aiPieces;
            bPieces = playerPieces;
            */
            aiScore += aiPieces;
            playerScore += playerPieces;
            int total = aiScore - playerScore;
            return total;
        }

        public static int evaluateSide(int player)
        {
            ScoreTable sTable;
            if (player == Board.aiColor)
            {
                sTable = new ScoreAI();
            }
            else
            {
                sTable = new ScorePlayer();
            }
            int score, pieces;
            score = pieces = 0;
            for (int row = 0; row < eBoard.GetLength(0); row++)
            {
                for (int col = 0; col < eBoard.GetLength(0); col++)
                {
                    if (eBoard[row, col] * player == 1)
                    {
                        score += sTable.Pawn[row, col];
                        pieces += 100;
                    }
                    else if (eBoard[row, col] * player == 2)
                    {
                        score += sTable.Rook[row, col];
                        pieces += 500;
                    }
                    else if (eBoard[row, col] * player == 3)
                    {
                        score += sTable.Knight[row, col];
                        pieces += 300;
                    }
                    else if (eBoard[row, col] * player == 4)
                    {
                        score += sTable.Bishop[row, col];
                        pieces += 300;
                    }
                    else if (eBoard[row, col] * player == 5)
                    {
                        score += sTable.Queen[row, col];
                        pieces += 900;
                    }
                    else if (eBoard[row, col] * player == 6)
                    {
                        score += sTable.King[row, col];
                        pieces += 10000;
                    }
                }
            }
            if (player == Board.aiColor)
            {
                aiPieces = pieces;
            }
            else
            {
                playerPieces = pieces;
            }
            return score;
        }

        public static int evaluateBonus(IMove move, int depth)
        {
            if (move is Move)
            {
                Move m = (Move)move;
                if (m.killing.Piece != 0 && m.killing.Position != null)
                {
                    /*
                    Console.WriteLine("at depth " + depth + " applied bonus/penalty");
                    Console.WriteLine("for " + m.moving.Piece + " taking " + m.killing.Piece + "\n");
                     * */
                    return ScoreTable.PieceValue(m.killing.Piece)/10 * depth;
                }
            }
                return 0;
        }
    }

    abstract class ScoreTable
    {
        abstract public int[,] Pawn { get; }
        abstract public int[,] Rook { get; }
        abstract public int[,] Knight { get; }
        abstract public int[,] Bishop { get; }
        abstract public int[,] Queen { get; }
        abstract public int[,] King { get; }

        public static int PieceValue(int piece)
        {
            int p = Math.Abs(piece);
            if (p == 1)
            {
                return 100;
            }
            else if (p == 2)
            {
                return 500;
            }
            else if (p == 3)
            {
                return 300;
            }
            else if (p == 4)
            {
                return 325;
            }
            else if (p == 5)
            {
                return 900;
            }
            else if (p == 6)
            {
                return 10000;
            }
            else
            {
                return 0;
            }
        }
    }

    class ScoreAI : ScoreTable
    {
        public override int[,] Pawn
        {
            get { return pawn; }
        }

        public override int[,] Rook
        {
            get { return rook; }
        }

        public override int[,] Knight
        {
            get { return knight; }
        }

        public override int[,] Bishop
        {
            get { return bishop; }
        }

        public override int[,] Queen
        {
            get { return queen; }
        }

        public override int[,] King
        {
            get { return king; }
        }

        private int[,] pawn =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},    
            {7, 7, 13, 23, 26, 13, 7, 7},
            {-2, -2, 4, 12, 15, 4, -2, -2},
            {2, -3, 2, 9, 11, 2, -3, -3},
            {1, 2, 0, 6, 8, 0, -4, -4},
            {-4, -4, 2, 4, 6, 0, -4, -4},
            {-1, -1, 1, 5, 6, 1, -1, -1},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private int[,] rook =
        {
            {9, 9, 11, 10, 11, 9, 9, 9},
            {4, 6, 7, 9, 9, 7, 6, 4},
            {9, 10, 10, 11, 11, 10, 10, 9},
            {8, 8, 8, 9, 9, 8, 8, 8},
            {6, 6, 5, 6, 6, 5, 6, 6},
            {4, 5, 5, 5, 5, 5, 5, 4},
            {2, 3, 3, 5, 5, 3, 3, 2},
            {0, -2, 0, 5, 5, 3, 0, 0}
        };

        private int[,] knight =
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

        private int[,] bishop =
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

        private int[,] queen =
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

        private int[,] king =
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
    }

    class ScorePlayer : ScoreTable
    {
        public override int[,] Pawn
        {
            get { return pawn; }
        }

        public override int[,] Rook
        {
            get { return rook; }
        }

        public override int[,] Knight
        {
            get { return knight; }
        }

        public override int[,] Bishop
        {
            get { return bishop; }
        }

        public override int[,] Queen
        {
            get { return queen; }
        }

        public override int[,] King
        {
            get { return king; }
        }

        private int[,] pawn =
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

        private int[,] rook =
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

        private int[,] knight =
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

        private int[,] bishop =
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

        private int[,] queen =
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

        private int[,] king =
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
}
