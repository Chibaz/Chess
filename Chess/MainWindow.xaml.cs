using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board game;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void DrawBoard()
        {
            UIElementCollection tiles = grid.Children;
            foreach (UIElement t in tiles)
            {
                //Tile tile = t.
            }
        }
    }
}
