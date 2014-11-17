using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{

    public class Logic
    {
        private IMove next;
        private int depth = 5;
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
            doAlphaBeta(Board.Game, depth, Int32.MinValue, Int32.MaxValue, Board.aiColor);
            Console.WriteLine(count + " evaluations after " + total + " boards, score is " + score);
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
                    //move.Execute();
                    int v = doAlphaBeta(newBoard, rDepth - 1, alpha, beta, rPlayer*-1); //Recursive call on possible methods
                    //move.Undo();
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
                    //move.Execute();
                    int v = doAlphaBeta(newBoard, rDepth - 1, alpha, beta, rPlayer*-1); //Recursive call on possible methods
                    //move.Undo();
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
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5, 5, 10, 27, 27, 10, 5, 5},
            {0, 0, 0, 25, 25, 0, 0, 0},
            {5, -5, -10, 0, 0, -10, -5, 5},
            {5, 10, 10, -25, -25, 10, 10, 5},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[,] blackKnightTable =
        {
            {-50, -40, -20, -30, -30, -20, -40, -50},
            {-40, -20, 0, 5, 5, 0, -20, -40}, 
            {-30, 5, 10, 15, 15, 10, 5, -30},
            {-30, 0, 15, 20, 20, 15, 0, -30}, 
            {-30, 5, 15, 20, 20, 15, 5, -30}, 
            {-30, 0, 10, 15, 15, 10, 0, -30},
            {-40, -20, 0, 0, 0, 0, -20, -40},
            {-50, -40, -30, -30, -30, -30, -40, -50}
        };

        private static int[,] blackBishopTable =
        {
            {-20, -10, -10, -10, -10, -10, -10, -20},
            {-10, 5, 0, 0, 0, 0, 5, -10}, 
            {-10, 10, 10, 10, 10, 10, 10, -10},
            {-10, 0, 10, 10, 10, 10, 0, -10}, 
            {-10, 5, 5, 10, 10, 5, 5, -10}, 
            {-10, 0, 5, 10, 10, 5, 0, -10},
            {-10, 0, 0, 0, 0, 0, 0, -10}, 
            {-20, -10, -10, -10, -10, -10, -10, -20}
        };

        private static int[,] blackRookTable =
        {
            {0, 0, 0, 5, 5, 0, 0, 0},
            {-5, 0, 0, 0, 0, 0, 0, -5}, 
            {-5, 0, 0, 0, 0, 0, 0, -5}, 
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {5, 10, 10, 10, 10, 10, 10, 5},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[,] blackQueenTable =
        {
            {-20, -10, -10, -5, -5, -10, -10, -20}, 
            {-10, 0, 5, 0, 0, 0, 0, -10}, 
            {-10, 5, 5, 5, 5, 5, 0, -10},
            {0, 0, 5, 5, 5, 5, 0, -5}, 
            {-5, 0, 5, 5, 5, 5, 0, -5}, 
            {-10, 0, 5, 5, 5, 5, 0, -10},
            {-10, 0, 0, 0, 0, 0, 0, -10},
            {-20, -10, -10, -5, -5, -10, -10, -20}
        };

        private static int[,] blackKingTable =
        {
            {20, 30, 10, 0, 0, 10, 30, 20}, 
            {20, 20, 0, 0, 0, 0, 20, 20}, 
            {-10, -20, -20, -20, -20, -20, -20, -10},
            {-20, -30, -30, -40, -40, -30, -30, -20},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30}, 
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30}
        };

        private static int[,] blackPawnTable =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {5, 10, 10, -25, -25, 10, 10, 5},
            {5, -5, -10, 0, 0, -10, -5, 5},
            {0, 0, 0, 25, 25, 0, 0, 0},
            {5, 5, 10, 27, 27, 10, 5, 5},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[,] knightTable =
        {
            {-50, -40, -30, -30, -30, -30, -40, -50},
            {-40, -20, 0, 0, 0, 0, -20, -40},
            {-30, 0, 10, 15, 15, 10, 0, -30},
            {-30, 5, 15, 20, 20, 15, 5, -30}, 
            {-30, 0, 15, 20, 20, 15, 0, -30},
            {-30, 5, 10, 15, 15, 10, 5, -30},
            {-40, -20, 0, 5, 5, 0, -20, -40},
            {-50, -40, -20, -30, -30, -20, -40, -50}

        };

        private static int[,] bishopTable =
        {
            {-20, -10, -10, -10, -10, -10, -10, -20},
            {-10, 0, 0, 0, 0, 0, 0, -10}, 
            {-10, 0, 5, 10, 10, 5, 0, -10},
            {-10, 5, 5, 10, 10, 5, 5, -10}, 
            {-10, 0, 10, 10, 10, 10, 0, -10}, 
            {-10, 10, 10, 10, 10, 10, 10, -10},
            {-10, 5, 0, 0, 0, 0, 5, -10}, 
            {-20, -10, -10, -10, -10, -10, -10, -20}
        };

        private static int[,] rookTable =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {5, 10, 10, 10, 10, 10, 10, 5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5}, 
            {-5, 0, 0, 0, 0, 0, 0, -5}, 
            {0, 0, 0, 5, 5, 0, 0, 0}
        };

        private static int[,] queenTable =
        {
            {-20, -10, -10, -5, -5, -10, -10, -20},
            {-10, 0, 0, 0, 0, 0, 0, -10},
            {-10, 0, 5, 5, 5, 5, 0, -10},
            {-5, 0, 5, 5, 5, 5, 0, -5}, 
            {0, 0, 5, 5, 5, 5, 0, -5}, 
            {-10, 5, 5, 5, 5, 5, 0, -10},
            {-10, 0, 5, 0, 0, 0, 0, -10}, 
            {-20, -10, -10, -5, -5, -10, -10, -20}

        };

        private static int[,] kingTable =
        {
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30}, 
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-20, -30, -30, -40, -40, -30, -30, -20},
            {-10, -20, -20, -20, -20, -20, -20, -10},
            {20, 20, 0, 0, 0, 0, 20, 20}, 
            {20, 30, 10, 0, 0, 10, 30, 20}
        };

        public int evaluate(Board toEvaluate)
        {
            int whitescore, blackscore;
            whitescore = blackscore = 0;
            int[,] tiles = toEvaluate.tiles;
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for(int col =0; col<tiles.GetLength(0); col++) {
                    if (tiles[row, col] == 1)
                    {
                        whitescore += pawnTable[row,col] + 100;
                    }
                    else if (tiles[row, col] == -1)
                    {
                        blackscore += blackPawnTable[row, col] + 100;
                    }
                    else if (tiles[row, col] == 2)
                    {
                        whitescore += rookTable[row, col] + 500;
                    }
                    else if (tiles[row, col] == -2)
                    {
                        blackscore += blackRookTable[row, col] + 500;
                    }
                    else if (tiles[row, col] == 3)
                    {
                        whitescore += knightTable[row, col] + 320;
                    }
                    else if (tiles[row, col] == -3)
                    {
                        blackscore += blackKnightTable[row, col] + 320;
                        }
                    else if (tiles[row, col] == 4)
                    {
                        whitescore += bishopTable[row, col] + 325;
                        }
                    else if (tiles[row, col] == -4)
                    {
                        blackscore += blackBishopTable[row, col] + 325;
                        }
                    else if (tiles[row, col] == 5)
                    {
                        whitescore += queenTable[row,col] + 999;
                        }
                    else if (tiles[row, col] == -5)
                    {
                        blackscore += blackQueenTable[row, col] + 999;
                        }
                    else if (tiles[row, col] == 6)
                    {
                        whitescore += kingTable[row,col] + 11111;
                        }
                    else if (tiles[row, col] == -6)
                    {
                        blackscore += blackKingTable[row, col] + 11111;
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


