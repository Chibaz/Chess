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
        private int moveX, moveY;
        public Boolean moving;

        public MainWindow()
        {
            InitializeComponent();
            game = new Board(this);
            DrawBoard();
        }

        public void DrawBoard()
        {
            Tile[,] tiles = game.GetTiles();
            foreach (Tile t in tiles)
            {
                if (t.Owner != null)
                {
                    String pos = "c" + t.Y + t.X;
                    TextBlock block = (TextBlock)this.FindName(pos);
                    block.Text = GetUnicode(t, block.Background);
                }
                else
                {
                    String pos = "c" + t.Y + t.X;
                    TextBlock block = (TextBlock)this.FindName(pos);
                    block.Text = "";
                }
            }
        }

        public String GetUnicode(Tile tile, Brush color)
        {
            Piece piece = tile.Owner;
            String c = new BrushConverter().ConvertToString(color);
            String uni = null;
            if ((c.Equals("#FFFFFFFF") && piece.Owner) || (c.Equals("#FF000000") && !piece.Owner))
            {
                if (piece.Name == "pawn")
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
            else if ((c.Equals("#FF000000") && piece.Owner) || (c.Equals("#FFFFFFFF") && !piece.Owner))
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
            if (!moving)
            {
                /*Border b = new Border();
                b.Child = s;*/
                UIElement uie = s;
                uie.Effect = 
                new BlurEffect
                {
                    //GlowColor = new Color {A = 255, R = 255, G = 255, B = 0},
                    //GlowSize = 320,
                };
                game.MovePieceA(y-1, x-1);
            }
            else
            {
                int[] org = game.MovePieceB(y - 1, x - 1);
                TextBlock o = (TextBlock)this.FindName("c" + org[0] + "" + org[1]);
                UIElement uie = s;
                uie.Effect = null;
                DrawBoard();
            }
        }
    }
}