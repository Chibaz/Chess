using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public interface IMove
    {
        void Execute();
        //void ExecuteOnBoard(Board temp);
        void Undo();
    }

    public struct MovingPiece
    {
        public int[] Origin;
        public int[] Target;
        public int Piece;

        public MovingPiece(int[] origin, int[] target, int piece)
        {
            Origin = origin;
            Target = target;
            Piece = piece;
        }
    }

    public struct TakenPiece
    {
        public int[] Position;
        public int Piece;

        public TakenPiece(int[] position, int piece)
        {
            Position = position;
            Piece = piece;
        }
    }

    public class Move : IMove
    {
        public MovingPiece moving;
        public TakenPiece killing;

        public void Move()
        {

        }

        public void Execute()
        {
            int[,] tiles = Board.Game.Tiles;
            if(killing.Position != null){
                tiles[killing.Position[0], killing.Position[1]] = 0;
            }
            tiles[moving.Target[0], moving.Target[1]] = moving.Piece;
            tiles[moving.Origin[0], moving.Origin[0]] = 0;
        }

        public void Undo()
        {
            int[,] tiles = Board.Game.Tiles;
            tiles[moving.Origin[0], moving.Origin[0]] = moving.Piece;
            tiles[moving.Target[0], moving.Target[1]] = 0;
            if (killing.Position != null)
            {
                tiles[killing.Position[0], killing.Position[1]] = killing.Piece;
            }
        }

        /*
        private int[] origin;
        public int[] Origin { get { return origin; } }
        private int[] target;
        public int[] Target { get { return target; } set { target = value; } }
        private int toMove;
        public int ToMove { get { return toMove; } set { toMove = value; } }
        private int toKill;
        public int ToKill { get { return toKill; } set { toKill = value; } }

        public Move(int[] origin)
        {
            this.origin = origin;
            toMove = Board.Game.Tiles[origin[0], origin[1]];
        }

        public void Execute()
        {
            int[,] tiles = Board.Game.Tiles;
            toKill = tiles[target[0], target[1]];
            tiles[target[0], target[1]] = toMove;
            tiles[origin[0], origin[1]] = 0;
        }
        
        public void ExecuteOnBoard(Board temp)
        {
            int[,] tiles = temp.Tiles;
            if (toKill != null)
            {
                //tiles[toKill[0], toKill[1]] = 0;
            }
            tiles[target[0], target[1]] = toMove;
            tiles[origin[0], origin[1]] = 0;
        }

        public void Undo()
        {
            int[,] tiles = Board.Game.Tiles;
            tiles[target[0], target[1]] = toKill;
            tiles[origin[0], origin[1]] = toMove;
        }
        */
    }

    public class Castling : IMove
    {

        /*
        private int[] origin;
        public int[] Origin { get { return origin; } }
        private int[] target;
        public int[] Target { get { return target; } set { target = value; } }
        private int[] toKill;
        public int[] ToKill { get { return toKill; } set { toKill = value; } }

        public Castling(int[] origin)
        {

        }

        public void Execute()
        {

        }
        public void ExecuteOnBoard(Board temp)
        {

        }
        public void Undo()
        {

        }
        */
    }
}
