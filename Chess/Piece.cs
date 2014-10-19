using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class Piece
    {
        protected string name;
        public string Name{
            get { return name; }
        }
        protected Boolean owner;
        public Boolean Owner
        {
            get { return owner; } 
        }
        protected Boolean movement;
        public Boolean Movement
        {
            get { return movement; }
        }
        protected int[,] moveAbs { get; set; } //Used for pieces with absolute movement lengths
        protected String[] moveVar { get; set; } //Used for pieces with variable movement lengths
        protected int[,] specials { get; set; }

        /*
        public List<int[]> generateMoves(int[] position)
        {
            List<int[]> realMoves = new List<int[]>();
            if(moveVar.Equals(null))
            foreach(int[] move in moveVar){
                realMoves.Add(new int[] {position[0]+move[0], position[1]+move[1]});
            }
            else{

            }
            return realMoves;
        }
        */
    }

    public class Pawn : Piece
    {
        public Pawn(Boolean owner)
        {
            name = "pawn";
            this.owner = owner;
            movement = false;
            if (owner)
            {
                moveAbs = new int[,] { { 1, 0 } };
                specials = new int[,] { { 1, -1 }, {-1, 1} };
            }
            else
            {
                moveAbs = new int[,] { { -1, 0 } };
                specials = new int[,] { { -1, -1 }, { -1, 1 } };
            }
        }
    }

    public class Rook : Piece
    {
        public Rook(Boolean owner){
            name = "rook";
            this.owner = owner;
            movement = true;
            //moveAbs = false;
            moveVar = new String[] { "straight" };
        }
    }

    public class Knight : Piece
    {
        public Knight(Boolean owner)
        {
            name = "knight";
            this.owner = owner;
            movement = false;
            moveAbs = new int[,] { {2, 1}, {2, -1}, {-2, 1}, {-2, -1}, {1, 2}, {1, -2}, {-1, 2}, {-1, -2} };
        }
    }

    public class Bishop : Piece
    {
        public Bishop(Boolean owner)
        {
            name = "bishop";
            this.owner = owner;
            movement = true;
            //moveAbs = false;
            moveVar = new String[] { "diagonal" };
        }
    }
    public class King : Piece
    {
        public King(Boolean owner)
        {
            name = "king";
            this.owner = owner;
            movement = false;
            moveAbs = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };                 
        }
    }

    public class Queen : Piece
    {
        public Queen(Boolean owner)
        {
            name = "queen";
            this.owner = owner;
            movement = true;
            //moveAbs = true;
            moveVar = new String[] { "straight", "diagonal" };
        }
    }
}
