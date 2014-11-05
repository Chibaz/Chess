using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    public class Logic
    {


        public int doAlphaBeta(Board temp, int rDepth, int alpha, int beta, int rPlayer)
        {
            //List<Board> childs = temp.getChilds(rPlayer);
            if (!childs.Any() || rDepth == 0)
            {
                int e = evaluate(temp);
                return e;
            }
            if (rPlayer == 1) //Maximizing
            {
                //Get all possible moves from current state
                foreach (Board child in childs) 
                {
                    int v = doAlphaBeta(child, rDepth - 1, alpha, beta, -1); //Recursive call on possible methods
                    if (v > alpha)
                    {
                        alpha = v;
                        if (rDepth == depth)
                        {
                            next = new Board(child.cloneBoard());
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
                foreach (Board child in childs)
                {
                    int v = doAlphaBeta(child, rDepth - 1, alpha, beta, 1); //Recursive call on possible methods
                    if (v < beta)
                    {
                        beta = v;
                        if (rDepth == depth)
                        {
                            next = new Board(child.cloneBoard());
                        }
                    }
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return beta;
            }
        }

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
        }
    }
    }
}
