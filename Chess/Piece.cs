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
        protected Boolean color;
        public Boolean Color
        {
            get { return color; } 
        }
        protected Boolean movement;
        public Boolean Movement
        {
            get { return movement; }
        }
        protected String[] move;
        public String[] Move
        { 
            get { return move; }
        }
        protected int[,] specials { get; set; }
    }

    public class Pawn : Piece
    {
        public Pawn(Boolean owner, Boolean startDir)
        {
            name = "pawn";
            this.color = owner;
            movement = false;
            if (!startDir)
            {
                move = new String[] { "1,0" };
                specials = new int[,] { { 1, -1 }, {-1, 1} };
            }
            else
            {
                move = new String[] { "-1,0" };
                specials = new int[,] { { -1, -1 }, { -1, 1 } };
            }
        }
    }

    public class Rook : Piece
    {
        public Rook(Boolean owner){
            name = "rook";
            this.color = owner;
            movement = true;
            //moveAbs = false;
            move = new String[] { "straight" };
        }
    }

    public class Knight : Piece
    {
        public Knight(Boolean owner)
        {
            name = "knight";
            this.color = owner;
            movement = false;
            move = new String[] { "2,1", "2,-1", "-2,1", "-2,-1", "1,2", "1,-2", "-1,2", "-1,-2" };
        }
    }

    public class Bishop : Piece
    {
        public Bishop(Boolean owner)
        {
            name = "bishop";
            this.color = owner;
            movement = true;
            //moveAbs = false;
            move = new String[] { "diagonal" };
        }
    }
    public class King : Piece
    {
        public King(Boolean owner)
        {
            name = "king";
            this.color = owner;
            movement = false;
            move = new String[] { "1,0", "-1,0", "0,1", "0,-1", "1,1", "1,-1", "-1,1", "-1,-1" };                 
        }
    }

    public class Queen : Piece
    {
        public Queen(Boolean owner)
        {
            name = "queen";
            this.color = owner;
            movement = true;
            //moveAbs = true;
            move = new String[] { "straight", "diagonal" };
        }
    }
}
