using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{

    public class Logic
    {
        private IMove next;
        private int depth = 3;
        private int score, count, total;
        private MoveGenerator mg;

        public Logic()
        {
            mg = new MoveGenerator();
        }

        public void GetBestMove()
        {
            Console.WriteLine("performing best move");
            Board orgBoard = Board.Game.CloneBoard();
            count = score = total = 0;
            Stopwatch time = new Stopwatch();
            time.Start();
            doAlphaBeta(Board.Game, depth, Int32.MinValue, Int32.MaxValue, Board.aiColor);
            time.Stop();
            Console.WriteLine(time.Elapsed + " " + count + " evaluations after " + total + " boards, score is " + score);
            next.Execute();
        }
        
        public int doAlphaBeta(Board lastBoard, int rDepth, int alpha, int beta, int rPlayer)
        {
            List<IMove> newMoves = mg.GetAllMovesForPlayer(rPlayer);
            total += newMoves.Count;
            //test
            //Console.WriteLine("number of moves from last board: " + newMoves.Count + " at depth " + rDepth + " for player + " + rPlayer);
            if (!newMoves.Any() || rDepth == 0)
            {
                int e = evaluate(lastBoard);
                if (e > score)
                {
                    Console.WriteLine("new best evaluation " + e);
                    score = e;
                }
                return e;
            }
            if (rPlayer == 1) //Maximizing
            {
                //Get all possible moves from current state
                foreach (IMove move in newMoves)
                {
                    Board newBoard = lastBoard.CloneBoard();
                    move.ExecuteOnBoard(newBoard);
                    int v = doAlphaBeta(newBoard, rDepth - 1, alpha, beta, rPlayer*-1); //Recursive call on possible methods
                    if (v > alpha)
                    {
                        alpha = v;
                        if (rDepth == depth)
                        {
                            Console.WriteLine("new best move " + v);
                            next = move;
                        }
                    }
                    if (alpha >= beta) //Stop if alpha is equal to or higher than beta, and prune the remainder
                    {
                        break;
                    }
                }
                return alpha;
            }
            else //Minimizing
            {
                foreach (IMove move in newMoves)
                {
                    Board newBoard = lastBoard.CloneBoard();
                    move.ExecuteOnBoard(newBoard);
                    int v = doAlphaBeta(newBoard, rDepth - 1, alpha, beta, rPlayer*-1); //Recursive call on possible methods
                    if (v < beta)
                    {
                        beta = v;
                        if (rDepth == depth)
                        {
                            next = move;
                        }
                    }
                    if (alpha >= beta) //Stop if alpha is equal to or higher than beta, and prune the remainder
                    {
                        break;
                    }
                }
                return beta;
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
            {0, 0, 0, 0, 0, 0, 0, 0}
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
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        public int evaluate(Board toEvaluate)
        {
            int whitescore, blackscore;
            whitescore = blackscore = 0;
            int[,] tiles = toEvaluate.tiles;
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for(int col =0; col<tiles.GetLength(0); col++) {
                    if (tiles[row, col] * Board.aiColor == 1)
                    {
                        whitescore += pawnTable[row,col] + 100;
                    }
                    else if (tiles[row, col] * Board.aiColor == -1)
                    {
                        blackscore += blackPawnTable[row, col] + 100;
                    }
                    else if (tiles[row, col] * Board.aiColor == 2)
                    {
                        whitescore += rookTable[row, col] + 500;
                    }
                    else if (tiles[row, col] * Board.aiColor == -2)
                    {
                        blackscore += blackRookTable[row, col] + 500;
                    }
                    else if (tiles[row, col] * Board.aiColor == 3)
                    {
                        whitescore += knightTable[row, col] + 300;
                    }
                    else if (tiles[row, col] * Board.aiColor == -3)
                    {
                        blackscore += blackKnightTable[row, col] + 300;
                        }
                    else if (tiles[row, col] * Board.aiColor == 4)
                    {
                        whitescore += bishopTable[row, col] + 300;
                        }
                    else if (tiles[row, col] * Board.aiColor == -4)
                    {
                        blackscore += blackBishopTable[row, col] + 300;
                        }
                    else if (tiles[row, col] * Board.aiColor == 5)
                    {
                        whitescore += queenTable[row,col] + 900;
                        }
                    else if (tiles[row, col] * Board.aiColor == -5)
                    {
                        blackscore += blackQueenTable[row, col] + 900;
                        }
                    else if (tiles[row, col] * Board.aiColor == 6)
                    {
                        whitescore += kingTable[row,col] + 10000;
                        }
                    else if (tiles[row, col] * Board.aiColor == -6)
                    {
                        blackscore += blackKingTable[row, col] + 10000;
                    }
                }
            }
            count++;

            //Console.WriteLine("Iteration " + count + " White Score: " + whitescore + " Black Score: " + blackscore);

            return whitescore - blackscore;
        }
        
    }
        /*
        public int evaluate(Board b)
        {
            count++;
            int score = 0;
            score += evaluateLine(b, 0, 0, 0, 1, 0, 2);
            score += evaluateLine(b, 1, 0, 1, 1, 1, 2);
            score += evaluateLine(b, 2, 0, 2, 1, 2, 2);
            score += evaluateLine(b, 0, 0, 1, 0, 2, 0);
            score += evaluateLine(b, 0, 1, 1, 1, 2, 1);
            score += evaluateLine(b, 0, 2, 1, 2, 2, 2);
            score += evaluateLine(b, 0, 0, 1, 1, 2, 2);
            score += evaluateLine(b, 0, 2, 1, 1, 2, 0);
            //Console.WriteLine(b.toString());
            //Console.WriteLine("evaluation: " + score);
            //Console.ReadLine();
            return score;
        }

        public int evaluateLine(Board b, int c1, int r1, int c2, int r2, int c3, int r3)
        {
            int score = 0;
            int[,] board = b.cloneBoard();

            if (board[c1, r1] == computer)
            {
                score = 1;
            }
            else if (board[c1, r1] == player)
            {
                score = -1;
            }

            if (board[c2, r2] == computer)
            {
                if (score == 1)
                {
                    score = 10;
                }
                else if (score == -1)
                {
                    return 0;
                }
                else
                {
                    score = 1;
                }
            }
            else if (board[c2, r2] == player)
            {
                if (score == -1)
                {
                    score = -10;
                }
                else if (score == 1)
                {
                    return 0;
                }
                else
                {
                    score = -1;
                }
            }

            if (board[c3, r3] == computer)
            {
                if (score > 0)
                {
                    score *= 20;
                }
                else if (score < 0)
                {
                    return 0;
                }
                else
                {
                    score = 1;
                }
            }
            else if (board[c3, r3] == player)
            {
                if (score < 0)
                {
                    score *= 10;
                }
                else if (score > 0)
                {
                    return 0;
                }
                else
                {
                    score = -1;
                }
            }
            return score;
        }*/
    }


