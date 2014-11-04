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
        public Boolean moving;
        private Move nextMove;

        public MainWindow()
        {
            InitializeComponent();
            game = new Board(this);
            game.resetGame(-1);
            DrawBoard();
        }

        //Goes through the tiles in the board and puts the pieces in the correct places
        public void DrawBoard()
        {
            int[,] tiles = game.GetTiles();
            foreach (int p in tiles)
            {
                String pos = "c" + t.Y + t.X;
                TextBlock block = (TextBlock)this.FindName(pos);
                if (t.Owner != null)
                {
                    block.Text = GetUnicode(t, block.Background);
                }
                else
                {
                    block.Text = "";
                }
            }
        }

        //Gets the unicode string used for symbolizing any specific piece
        public String GetUnicode(int piece, Brush color)
        {
            String c = new BrushConverter().ConvertToString(color);
            String uni = null;
            if ((c.Equals("#FFFFFFFF") && piece.Color) || (c.Equals("#FF000000") && !piece.Color))
            {
                if (piece == 1)
                {
                    uni = "\u2659";
                }
                else if (piece.Name == "knight")
                {
                    uni = "\u2658";
                }
                else if (piece.Name == "bishop")
                {
                    uni = "\u2657";
                }
                else if (piece.Name == "rook")
                {
                    uni = "\u2656";
                }
                else if (piece.Name == "queen")
                {
                    uni = "\u2655";
                }
                else if (piece.Name == "king")
                {
                    uni = "\u2654";
                }
            }
            else if ((c.Equals("#FF000000") && piece.Color) || (c.Equals("#FFFFFFFF") && !piece.Color))
            {
                if (piece.Name == "pawn")
                {
                    uni = "\u265F";
                }
                else if (piece.Name == "knight")
                {
                    uni = "\u265E";
                }
                else if (piece.Name == "bishop")
                {
                    uni = "\u265D";
                }
                else if (piece.Name == "rook")
                {
                    uni = "\u265C";
                }
                else if (piece.Name == "queen")
                {
                    uni = "\u265B";
                }
                else if (piece.Name == "king")
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
            Tile clicked = game.GetSpecificTile(y, x);
            if (nextMove == null && clicked.Owner != null)
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
                }
            }
        }
    }
}