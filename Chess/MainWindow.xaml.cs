using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board game;
        private Move nextMove;

        public MainWindow()
        {
            InitializeComponent();
            game = new Board();
            game.resetGame(-1);
            DrawBoard();
        }

        //Goes through the tiles in the board and puts the pieces in the correct places
        public void DrawBoard()
        {
            int[,] tiles = game.GetTiles();
            for (int h = 0; h < 8; h++)
            {
                for (int w = 0; w < 8; w++)
                {
                    int piece = tiles[h, w];
                    String pos = "c" + h + w;
                    TextBlock block = (TextBlock)this.FindName(pos);
                    if (piece != 0)
                    {
                        block.Text = GetUnicode(piece, block.Background);
                    }
                    else
                    {
                        block.Text = "";
                    }
                }
            }
        }

        //Gets the unicode string used for symbolizing any specific piece
        public String GetUnicode(int piece, Brush color)
        {
            String c = new BrushConverter().ConvertToString(color);
            String uni = null;
            int absPiece = Math.Abs(piece);
            if ((c.Equals("#FFFFFFFF") && piece > 0) || (c.Equals("#FF000000") && piece < 0))
            {
                if (absPiece == 1)
                {
                    uni = "\u2659";
                }
                else if (absPiece == 3)
                {
                    uni = "\u2658";
                }
                else if (absPiece == 4)
                {
                    uni = "\u2657";
                }
                else if (absPiece == 2)
                {
                    uni = "\u2656";
                }
                else if (absPiece == 5)
                {
                    uni = "\u2655";
                }
                else if (absPiece == 6)
                {
                    uni = "\u2654";
                }
            }
            else if ((c.Equals("#FF000000") && piece > 0) || (c.Equals("#FFFFFFFF") && piece < 0))
            {
                if (absPiece == 1)
                {
                    uni = "\u265F";
                }
                else if (absPiece == 3)
                {
                    uni = "\u265E";
                }
                else if (absPiece == 4)
                {
                    uni = "\u265D";
                }
                else if (absPiece == 2)
                {
                    uni = "\u265C";
                }
                else if (absPiece == 5)
                {
                    uni = "\u265B";
                }
                else if (absPiece == 6)
                {
                    uni = "\u265A";
                }
            }
            return uni.ToString();
        }

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            TextBlock s = (TextBlock)sender;
            int y = Int32.Parse(s.Name.Substring(1, 1));
            int x = Int32.Parse(s.Name.Substring(2, 1));
            Console.WriteLine("selected: " + y + ":" + x);
            int[] clicked = new int[] { y, x };//game.GetSpecificTile(y, x);
            if (nextMove == null && game.GetSpecificTile(clicked[0], clicked[1]) != 0)
            {
                UIElement uie = s;
                uie.Effect = new BlurEffect
                {
                    //GlowColor = new Color {A = 255, R = 255, G = 255, B = 0},
                    //GlowSize = 320,
                };
                nextMove = new Move(clicked);
                //game.MovePieceA(y-1, x-1);
                Console.WriteLine("move started");
            }
            else if (nextMove != null && clicked == nextMove.Org)
            {
                nextMove = null;
                UIElement uie = s;
                uie.Effect = null;
            }
            else if (nextMove != null)
            {
                /*
                if (game.GetLegalMovements(nextMove.Org).Contains(clicked))
                {
                    Console.WriteLine("move is legal");
                    nextMove.Target = clicked;
                    //int[] org = game.MovePieceB(y - 1, x - 1);
                    //TextBlock o = (TextBlock)this.FindName("c" + org[0] + "" + org[1]);
                    nextMove.Execute();
                    UIElement uie = s;
                    uie.Effect = null;
                    DrawBoard();
                    nextMove = null;
                }
                else
                {
                    Console.WriteLine("move is illegal");
                }*/
            }
        }
    }
}