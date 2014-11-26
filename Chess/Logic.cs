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
        private int score, evals, total;
        private MoveGenerator mg;
        public int depth = 3;
        public Boolean endGame;

        public Logic()
        {
            mg = new MoveGenerator();
        }

        public void GetBestMove()
        {
            Console.WriteLine("performing best move");
            //Board orgBoard = Board.Game.CloneBoard();
            evals = score = total = 0;
            Stopwatch time = new Stopwatch();
            //depth = 0;
            time.Start();
            Move bestMove = null;
            /*while (time.Elapsed < timeAllowed)
            {*/
            doAlphaBeta(Board.Game, depth, Int32.MinValue, Int32.MaxValue, Board.aiColor, bestMove, 0);
            /*depth++;
        }*/
            time.Stop();
            Console.WriteLine(time.Elapsed + ": " + evals + " evaluations after " + total + " boards");
            next.Execute();
        }

        public int doAlphaBeta(Board lastBoard, int rDepth, int alpha, int beta, int rPlayer, Move prioMove, int bonus)
        {
            List<IMove> newMoves = mg.GetAllMovesForPlayer(lastBoard, rPlayer);
            total += newMoves.Count;
            int newBonus = bonus;
            //Console.WriteLine("number of moves from last board: " + newMoves.Count + " at depth " + rDepth + " for player + " + rPlayer);
            if (!newMoves.Any() || rDepth == 0)
            {
                int e = Evaluation.evaluate(lastBoard);
                Console.WriteLine("eval: " + e + " bonus is " + bonus);
                //e += Evaluation.evaluateBonus(new Move(new int[] { 0, 0 }, 0), rDepth);//, rDepth
                /*if (bonus != 0)
                {
                    Console.WriteLine("added " + bonus + " to evaluation");
                }
                Console.WriteLine("total is " + (e + bonus));*/
                evals++;
                //Console.WriteLine("eval " + evals);
                return e + bonus;
            }
            if (rPlayer == Board.aiColor) //Maximizing
            {
                //Get all possible moves from current state
                foreach (IMove move in newMoves)
                {
                    Board newBoard = lastBoard.CloneBoard();
                    move.ExecuteOnBoard(newBoard);
                    /*
                    if (rDepth == 1)
                    {
                        Console.WriteLine("lastBoard was");
                        foreach (var entry in lastBoard.tiles)
                        {
                            Console.Write(entry + " ");
                        }
                        Console.WriteLine("newBoard is");
                        foreach (var entry in newBoard.tiles)
                        {
                            Console.Write(entry + " ");
                        }
                        Console.WriteLine();
                    }*/
                    newBonus = bonus + Evaluation.evaluateBonus(move, rDepth);
                    int v = doAlphaBeta(newBoard, rDepth - 1, alpha, beta, rPlayer * -1, null, newBonus); //Recursive call on possible methods
                    if (v > alpha)
                    {
                        alpha = v;
                        if (rDepth == depth)
                        {
                            //Console.WriteLine("new best move " + v);
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
                    /*
                    if (rDepth == 1)
                    {
                        Console.WriteLine("lastBoard was");
                        foreach (var entry in lastBoard.tiles)
                        {
                            Console.Write(entry + " ");
                        }
                        Console.WriteLine("newBoard is");
                        foreach (var entry in newBoard.tiles)
                        {
                            Console.Write(entry + " ");
                        }
                        Console.WriteLine();
                    }
                    if (move is Move && ((Move)move).killing.Position != null)
                    {
                        newBonus -= 100 * rDepth;
                        Console.WriteLine("kill penalty");
                        //Console.WriteLine("bonus of " + newBonus + " applied");
                    }*/
                    /*if (Evaluation.evaluateBonus(move, rDepth) != 0)
                    {
                        Console.WriteLine("increased bonus from " + bonus + " to " + newBonus);
                    }*/
                    newBonus = bonus - Evaluation.evaluateBonus(move, rDepth);
                    int v = doAlphaBeta(newBoard, rDepth - 1, alpha, beta, rPlayer * -1, null, newBonus); //Recursive call on possible method                    
                    if (v < beta)
                    {
                        beta = v;
                    }
                    if (alpha >= beta) //Stop if alpha is equal to or higher than beta, and prune the remainder
                    {
                        break;
                    }
                }
                return beta;
            }
        }
    }
}


